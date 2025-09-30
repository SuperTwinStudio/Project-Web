using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Botpa;
using System.Reflection;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public enum MenuTransition { None, Fade }

public class MenuManager : MonoBehaviour {

    //Events
    public delegate void MenuEvent(string oldMenu, string newMenu);
    
    private event MenuEvent OnMenuChanged;
    private event MenuEvent OnTransitionStart;
    private event MenuEvent OnTransitionEnd;

    //Level
    [Header("Level")]
    [SerializeField] private Level _level;

    public Level Level => _level;

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference backAction;

    //Master
    [Header("UI")]
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Animator menuTransitions;

    public Canvas MainCanvas => _mainCanvas;

    public bool InTransition { get; private set; }

    public const float TRANSITION_DURATION = 1/3f;

    //Menus
    [Header("Menus")]
    [SerializeField] private Transform menusParent;
    
    [HideInInspector, SerializeField] private List<Menu> menus = new();
    [HideInInspector, SerializeField] private SerializableDictionary<string, Menu> createdMenus = new();

    private bool HasMenu => menus.Count > 0;
    private int CurrentMenuIndex => menus.Count - 1;
    private Menu CurrentMenu => HasMenu ? menus[CurrentMenuIndex] : null;

    public string CurrentMenuName => HasMenu ? CurrentMenu.Name : MenusList.None;


    //State
    public void Init(MenuManager other) {
        //Check if other is this one
        if (other != this) {
            //Init other MenuManager -> Close menus & open new ones
            CloseAll();
            foreach (var menu in other.menus) Open(menu.Name);
        } else if (HasMenu) {
            //Reopen all menus
            for (int i = 0; i < menus.Count; i++) {
                menus[i].Open(this, i);
                if (i > 0) menus[i - 1].Cover();
            }
        }
    }

    private void Update() {
        //Do not update if in transition
        if (InTransition) return;

        //Execute action in menus
        if (HasMenu) {
            //Back button pressed -> Perform back in menu, then close if returned true
            if (backAction.Triggered() && CurrentMenu.OnBack()) CloseLast();
            //Update menu
            else CurrentMenu.OnUpdate();
        }
    }

    //Open
    public void Open(string name, object args = null) {
        Open(name, MenuTransition.None, args);
    }

    public void Open(string name, MenuTransition transition, object args = null) {
        //In transition -> Can't change menus
        if (InTransition) return;

        //A menu of the same type is already open -> Return
        if (IsOpen(name)) return;

        //Open
        switch (transition) {
            //No transition
            case MenuTransition.None:
                OnOpen(name, args);
                break;

            //Transition
            default:
                InTransition = true;
                StartCoroutine(OpenCoroutine(name, transition, args));
                break;
        }
    }

    private IEnumerator OpenCoroutine(string newMenu, MenuTransition transition, object args) {
        //Events
        string oldMenu = CurrentMenuName;
        OnTransitionStart?.Invoke(oldMenu, newMenu);

        //Transition
        switch (transition) {
            //Fade
            case MenuTransition.Fade:
                menuTransitions.SetTrigger("FadeOut");
                yield return new WaitForSeconds(TRANSITION_DURATION);
                OnOpen(newMenu, args);
                menuTransitions.SetTrigger("FadeIn");
                yield return new WaitForSeconds(TRANSITION_DURATION);
                break;
        }
        InTransition = false;

        //Events
        OnTransitionEnd?.Invoke(oldMenu, newMenu);
    }

    private void OnOpen(string newMenu, object args) {
        //Events
        string oldMenu = CurrentMenuName;

        //Get current menu
        CurrentMenu?.Cover();

        //Add new menu
        Menu menu = GetMenu(newMenu);
        menus.Add(menu);
        menu.Open(this, CurrentMenuIndex, args);

        //Events
        OnMenuChanged?.Invoke(oldMenu, newMenu);
    }

    public bool IsOpen(string name) {
        foreach (var menu in menus)
            if (menu.Name.Equals(name)) 
                return true;
        return false;
    }

    //Close
    public void CloseLast(MenuTransition transition = MenuTransition.None) {
        CloseLast(false, transition);
    }

    private void CloseLast(bool force, MenuTransition transition = MenuTransition.None) {
        //In transition -> Can't change menus
        if (!force && InTransition) return;

        //No menus -> Nothing to close
        if (!HasMenu) return;

        //Close last
        switch (transition) {
            //No transition
            case MenuTransition.None:
                OnCloseLast(transition);
                break;

            //Transition
            default:
                InTransition = true;
                StartCoroutine(CloseLastCoroutine(transition));
                break;
        }
    }

    private void OnCloseLast(MenuTransition transition = MenuTransition.None) {
        //Events
        string oldMenu = CurrentMenuName;

        //Get menu
        int menuIndex = CurrentMenuIndex;
        menus[menuIndex]?.Close();
        menus.RemoveAt(menuIndex);

        //Events
        string newMenu = CurrentMenuName;
        OnMenuChanged?.Invoke(oldMenu, newMenu);

        //Uncover previous menu
        if (menuIndex > 0) {
            menuIndex--;
            menus[menuIndex]?.Uncover();
        }
    }

    private IEnumerator CloseLastCoroutine(MenuTransition transition) {
        //Events
        string oldMenu = CurrentMenuName;
        string newMenu = menus.Count >= 2 ? menus[CurrentMenuIndex - 1].Name : "";
        OnTransitionStart?.Invoke(oldMenu, newMenu);

        //Transition
        switch (transition) {
            //Fade
            case MenuTransition.Fade:
                menuTransitions.SetTrigger("FadeOut");
                yield return new WaitForSeconds(TRANSITION_DURATION);
                OnCloseLast(transition);
                menuTransitions.SetTrigger("FadeIn");
                yield return new WaitForSeconds(TRANSITION_DURATION);
                break;
        }
        InTransition = false;

        //Events
        OnTransitionEnd?.Invoke(oldMenu, newMenu);
    }

    public void CloseAll() {
        //Close all menus
        for (int i = CurrentMenuIndex; i >= 0; i--) CloseLast(true);
    }

    public void ResetMenus() {
        //Close menus & clear menu objects
        CloseAll();
        createdMenus.Clear();
        Util.DestroyChildren(menusParent);
    }

    //Swap (close current & open new)
    public void Swap(string name, object args = null) {
        Swap(name, MenuTransition.None, args);
    }

    public void Swap(string name, MenuTransition transition, object args = null) {
        //In transition -> Can't change menus
        if (InTransition) return;

        //A menu of the same type is already open -> Return
        if (IsOpen(name)) return;

        //Swap
        switch (transition) {
            //No transition
            case MenuTransition.None:
                OnSwap(name, args);
                break;

            //Transition
            default:
                InTransition = true;
                StartCoroutine(SwapCoroutine(name, transition, args));
                break;
        }
    }

    private IEnumerator SwapCoroutine(string newMenu, MenuTransition transition, object args) {
        //Events
        string oldMenu = CurrentMenuName;
        OnTransitionStart?.Invoke(oldMenu, newMenu);

        //Transition
        switch (transition) {
            //Fade
            case MenuTransition.Fade:
                menuTransitions.SetTrigger("FadeOut");
                yield return new WaitForSeconds(TRANSITION_DURATION);
                OnSwap(newMenu, args);
                menuTransitions.SetTrigger("FadeIn");
                yield return new WaitForSeconds(TRANSITION_DURATION);
                break;
        }
        InTransition = false;

        //Events
        OnTransitionEnd?.Invoke(oldMenu, newMenu);
    }

    private void OnSwap(string newMenu, object args) {
        //Events
        string oldMenu = CurrentMenuName;

        //Close last menu
        if (HasMenu) {
            int menuIndex = CurrentMenuIndex;
            menus[menuIndex].Close();
            menus.RemoveAt(menuIndex);
        }

        //Open new menu
        Menu menu = GetMenu(newMenu);
        menus.Add(menu);
        menu.Open(this, CurrentMenuIndex, args);

        //Events
        OnMenuChanged?.Invoke(oldMenu, newMenu);
    }

    //Menu GameObjects
    public Menu GetMenu(string name) {
        //Check if object exists
        if (!createdMenus.ContainsKey(name) || !createdMenus[name]) {
            //Missing -> Create it
            GameObject obj;
            if (!Application.isPlaying) {
                //Instantiate prefab (in editor)
                #if UNITY_EDITOR
                obj = PrefabUtility.InstantiatePrefab(Menu.GetPrefabFromName(name), menusParent) as GameObject;
                Undo.RegisterCreatedObjectUndo(obj, "Created " + name + " menu");
                #endif
            } else {
                //Instantiate prefab (in game)
                obj = Instantiate(Menu.GetPrefabFromName(name), menusParent);
            }

            //Rename menu
            obj.name = name;
            createdMenus[name] = obj.GetComponent<Menu>();
        }

        //Return object
        return createdMenus[name];
    }

    public T GetMenu<T>() where T : Menu {
        return menus.OfType<T>().FirstOrDefault();
    }

    //Events
    public void AddOnMenuChanged(MenuEvent menuEvent) {
        OnMenuChanged += menuEvent;
    }

    public void RemoveOnMenuChanged(MenuEvent menuEvent) {
        OnMenuChanged -= menuEvent;
    }

    public void AddOnTransitionStart(MenuEvent menuEvent) {
        OnTransitionStart += menuEvent;
    }

    public void RemoveOnTransitionStart(MenuEvent menuEvent) {
        OnTransitionStart -= menuEvent;
    }

    public void AddOnTransitionEnd(MenuEvent menuEvent) {
        OnTransitionEnd += menuEvent;
    }

    public void RemoveOnTransitionEnd(MenuEvent menuEvent) {
        OnTransitionEnd -= menuEvent;
    }


    //Custom inspector
    #if UNITY_EDITOR
    [CustomEditor(typeof(MenuManager))]
    public class MenuManagerEditor : Editor {

        private MenuManager manager;
        private SerializedProperty menus;
        private SerializedProperty createdMenus;

        public void OnEnable() {
            manager = target as MenuManager;
            menus = serializedObject.FindProperty("menus");
            createdMenus = serializedObject.FindProperty("createdMenus");
        }

        public override void OnInspectorGUI() {
            //Update
            bool shouldSave = true;
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            //Default inspector
            DrawDefaultInspector();

            //Menus
            GUILayout.Space(10);
            GUILayout.Label("Open Menus", EditorStyles.boldLabel);
            for (int i = 0; i < manager.menus.Count; i++) GUILayout.Label("· " + (manager.menus[i] ? manager.menus[i].Name : "null"));

            //Menu buttons
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(18), GUILayout.Height(18))) {
                //Create menu
                GenericMenu menu = new();

                //Add entries to menu
                FieldInfo[] fields = typeof(MenusList).GetFields();
                foreach (var field in fields) {
                    //Get menu value
                    string value = (string) field.GetValue(null);
                    if (value.Equals(MenusList.None)) continue;

                    //Add item to menu
                    menu.AddItem(new GUIContent(field.Name), false, () => {
                        manager.Open(value);
                        Save($"Opened a menu in {manager.name} ({field.Name})", true);
                        shouldSave = false;
                    });
                }

                //Show menu
                menu.ShowAsContext();
            }
            if (GUILayout.Button("-", GUILayout.Width(18), GUILayout.Height(18))) {
                manager.CloseLast();
                Save("Closed last " + manager.name + " menu", true);
                shouldSave = false; //Don't save, as RemoveUnusedOverrides() won't work
            }
            if (GUILayout.Button("Reset", GUILayout.Width(60), GUILayout.Height(18))) {
                ResetMenus();
                Save("Reset " + manager.name + " menus", true);
                shouldSave = false; //Don't save, as RemoveUnusedOverrides() won't work
            }
            GUILayout.Space(10);
            GUILayout.Label("Presets:", GUILayout.Width(50));
            if (GUILayout.Button("Home", GUILayout.Width(70), GUILayout.Height(18))) {
                ResetMenus();
                manager.Open(MenusList.Home);
                Save($"Opened a menu in {manager.name} (Home)", true);
                shouldSave = false;
            }
            if (GUILayout.Button("Game", GUILayout.Width(70), GUILayout.Height(18))) {
                ResetMenus();
                manager.Open(MenusList.Game);
                Save($"Opened a menu in {manager.name} (Game)", true);
                shouldSave = false;
            }
            GUILayout.EndHorizontal();

            //Apply changes
            if (shouldSave && EditorGUI.EndChangeCheck()) Save("Modified " + manager.name, true);
        }

        private void ResetMenus() {
            manager.ResetMenus();
            PrefabUtility.RevertPropertyOverride(menus, InteractionMode.AutomatedAction);
            PrefabUtility.RevertPropertyOverride(createdMenus, InteractionMode.AutomatedAction);
        }

        private void Save(string change, bool registerUndo) {
            serializedObject.ApplyModifiedProperties();
            if (registerUndo && !Application.isPlaying) {
                Undo.RegisterCompleteObjectUndo(manager, change);
                EditorUtility.SetDirty(manager);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
    #endif
    
}
