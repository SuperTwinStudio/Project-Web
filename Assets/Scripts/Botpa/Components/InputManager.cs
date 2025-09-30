using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Botpa {

    public enum InputMethod { Keyboard, PlayStation, Xbox }

    public class InputManager : MonoBehaviour {
        
        //Singleton
        public static InputManager current { get; private set; }

        //Input methods
        private InputMethod inputMethod = InputMethod.Keyboard;

        private bool canSwitchInputMethod = true;

        public static bool useController => current.inputMethod != InputMethod.Keyboard;

        //Events
        private static event Action<InputMethod, InputMethod> onInputMethodChanged;

        //Input
        [Header("Any Input")]
        [SerializeField] private InputActionReference keyboardInput;
        [SerializeField] private InputActionReference playStationInput;
        [SerializeField] private InputActionReference xboxInput;

        //Icons
        [HideInInspector, SerializeField] private List<IconGroup> groupsList = new(1);
        [HideInInspector, SerializeField] private Dictionary<string, IconGroup> groups = new();


        //State
        public void Init() {
            //Already an input manager
            if (current) {
                //Is not self -> Destroy itself
                if (current != this) Destroy(gameObject);
                return;
            }

            //Set as current
            current = this;

            //Enable input
            if (keyboardInput) keyboardInput.Enable();
            if (playStationInput) playStationInput.Enable();
            if (xboxInput) xboxInput.Enable();

            //Prevent GameObject from being destroyed
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            //Init icons dictionary
            foreach (var group in groupsList) groups.TryAdd(group.name, group);

            //Get last used input method (use keyboard as default)
            InputMethod method = (InputMethod) Mathf.Clamp(PlayerPrefs.GetInt("Settings.InputMethod", (int) InputMethod.Keyboard), 0, Enum.GetValues(typeof(InputMethod)).Length - 1);

            //Check starting input type
            canSwitchInputMethod = true;
            switch (Application.platform) {
                //Windows (PC)
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    //Stay with the last used input (you can use keyboard or controller on PC)
                    break;

                //Linux & Steam Deck
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    if (SystemInfo.operatingSystem.Contains("SteamOS")) {
                        //Steam Deck
                        method = InputMethod.Xbox;
                        canSwitchInputMethod = false;
                    } else {
                        //Linux desktop
                    }
                    break;

                //PlayStation
                case RuntimePlatform.PS4:
                case RuntimePlatform.PS5:
                    //Force PS Controller
                    method = InputMethod.PlayStation;
                    canSwitchInputMethod = false;
                    break;

                //Other
                default:
                    //Use xbox controller in the rest
                    method = InputMethod.Xbox;
                    canSwitchInputMethod = false;
                    break;
            }

            //Select input method
            SelectInputMethod(method, true);
        }

        private void Awake() {
            //Init self
            Init();
        }

        private void Update() {
            //Check for input method changes
            if (!canSwitchInputMethod) return;

            //Check input
            if (keyboardInput && keyboardInput.IsPressed()) 
                SelectInputMethod(InputMethod.Keyboard);
            else if (playStationInput && playStationInput.IsPressed()) 
                SelectInputMethod(InputMethod.PlayStation);
            else if (xboxInput && xboxInput.IsPressed()) 
                SelectInputMethod(InputMethod.Xbox);
        }

        //Icons
        public static Sprite GetIcon(InputActionReference input) {
            string name = input.name;
            return current.groups.TryGetValue(name, out IconGroup group) ? group.icons[(int) current.inputMethod] : null;
        }

        //Input method
        private void SelectInputMethod(InputMethod newInputMethod, bool force = false) {
            //Same input -> Return
            if (!force && inputMethod == newInputMethod) return;

            //Save input method
            InputMethod oldInputMethod = inputMethod;
            inputMethod = newInputMethod;
            PlayerPrefs.SetInt("Settings.InputMethod", (int) inputMethod);

            //Notify method changed
            onInputMethodChanged?.Invoke(oldInputMethod, newInputMethod);
        }

        public static void AddOnInputMethodChanged(Action<InputMethod, InputMethod> action) {
            onInputMethodChanged += action;
        }

        public static void RemoveOnInputMethodChanged(Action<InputMethod, InputMethod> action) {
            onInputMethodChanged -= action;
        }



        //Icons group
        [Serializable]
        public class IconGroup {

            //Inspector
            #if UNITY_EDITOR
            public bool isOpen = false;
            #endif

            //Input & icons
            public InputActionReference input;
            public Sprite[] icons = new Sprite[Enum.GetValues(typeof(InputMethod)).Length];

            public string name => input != null ? input.name : "";

        }



        //Inspector
        #if UNITY_EDITOR
        [CustomEditor(typeof(InputManager)), CanEditMultipleObjects]
        public class InputManagerEditor : Editor {

            //Input manager
            private InputManager manager;

            //Icons & methods count
            private SerializedProperty groups;
            private int methodsCount;


            private void OnEnable() {
                //Get InputManager
                manager = target as InputManager;

                //Get icons list & methods count
                groups = serializedObject.FindProperty("groupsList");
                methodsCount = Enum.GetNames(typeof(InputMethod)).Length;

                //Set as current InputManager
                if (current != manager) {
                    current = manager;
                    serializedObject.ApplyModifiedProperties();
                }
            }

            public override void OnInspectorGUI() {
                //Update object
                serializedObject.Update();

                //Draw default editor
                DrawDefaultInspector();

                //Icons label
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Icons", EditorStyles.boldLabel);

                //Check for changes
                EditorGUI.BeginChangeCheck();

                //Create centered style for labels & calculate icons size
                var centeredStyle = GUI.skin.GetStyle("Label");
                centeredStyle.alignment = TextAnchor.UpperCenter;
                float iconSize = Mathf.Min((EditorGUIUtility.currentViewWidth - 28) / methodsCount, 100);

                //Draw groups
                int groupsCount = groups.arraySize;
                for (int groupIdx = 0; groupIdx < groupsCount; groupIdx++) {
                    //Get group & check if its open
                    SerializedProperty group = groups.GetArrayElementAtIndex(groupIdx);
                    bool isOpen = manager.groupsList[groupIdx].isOpen;

                    //Buttons & input action
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(isOpen ? "Hide" : "Show", GUILayout.Width(50), GUILayout.Height(20))) {
                        manager.groupsList[groupIdx].isOpen = !isOpen;
                    }
                    if (GUILayout.Button("✨", GUILayout.Width(20), GUILayout.Height(20))) {
                        int index = groupIdx;
                        GenericMenu menu = new();
                        menu.AddItem(new GUIContent("Add next"), false, () => { 
                            manager.groupsList.Insert(index + 1, new IconGroup());
                        });
                        menu.AddItem(new GUIContent("Remove"), false, () => { 
                            manager.groupsList.RemoveAt(index);
                        });
                        if (index > 0) {
                            menu.AddItem(new GUIContent("Move up"), false, () => { 
                                manager.groupsList.Swap(index, index - 1);
                            });
                        }
                        if (index < groupsCount - 1) {
                            menu.AddItem(new GUIContent("Move down"), false, () => {
                                manager.groupsList.Swap(index, index + 1);
                            });
                        }
                        menu.ShowAsContext();
                    }
                    EditorGUILayout.ObjectField(group.FindPropertyRelative("input"), GUIContent.none);
                    EditorGUILayout.EndHorizontal();

                    //Icons list
                    if (isOpen) {
                        //Icon names
                        EditorGUILayout.BeginHorizontal();
                        for (int methodIdx = 0; methodIdx < methodsCount; methodIdx++) {
                            EditorGUILayout.LabelField(Enum.GetName(typeof(InputMethod), methodIdx), centeredStyle, GUILayout.Width(iconSize));
                        }
                        EditorGUILayout.EndHorizontal();

                        //Get icons & fix array size
                        SerializedProperty icons = group.FindPropertyRelative("icons");
                        if (icons.arraySize != methodsCount) icons.arraySize = methodsCount;

                        //Draw icon sprites
                        EditorGUILayout.BeginHorizontal();
                        for (int j = 0; j < icons.arraySize; j++) {
                            EditorGUILayout.ObjectField(icons.GetArrayElementAtIndex(j), typeof(Sprite), GUIContent.none, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                //New group button
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20))) {
                    manager.groupsList.Insert(manager.groupsList.Count, new IconGroup());
                }
                EditorGUILayout.EndHorizontal();

                //End change check
                if (EditorGUI.EndChangeCheck()) {
                    serializedObject.ApplyModifiedProperties();
                    Undo.RecordObject(manager, $"Modified {typeof(InputMethod).Name} script");
                }
            }

        }
        #endif

    }

}

