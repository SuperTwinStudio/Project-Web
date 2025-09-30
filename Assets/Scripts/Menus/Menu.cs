using UnityEngine;

public enum MenuState { Closed, Open, Covered }

public class Menu : MonoBehaviour {

    //Managers
    [HideInInspector, SerializeField] private MenuManager _menus;

    public MenuManager MenuManager { get => _menus; private set => _menus = value; }
    public Level Level => MenuManager.Level;
    public Player Player => MenuManager.Level.Player;

    //Prefab
    public virtual string Name => "";
    public GameObject Prefab => GetPrefabFromName(Name);

    //Components
    private Canvas _canvas;

    public Canvas Canvas {
        get {
            if (!TryGetComponent(out _canvas)) Debug.LogError("Couldn't access Canvas");
            return _canvas;
        }
        private set => _canvas = value;
    }

    //State
    [HideInInspector, SerializeField] private MenuState _state = MenuState.Closed;

    public MenuState State { get => _state; private set => _state = value; }


      /*$$$$$   /$$                 /$$
     /$$__  $$ | $$                | $$
    | $$  \__//$$$$$$    /$$$$$$  /$$$$$$    /$$$$$$
    |  $$$$$$|_  $$_/   |____  $$|_  $$_/   /$$__  $$
     \____  $$ | $$      /$$$$$$$  | $$    | $$$$$$$$
     /$$  \ $$ | $$ /$$ /$$__  $$  | $$ /$$| $$_____/
    |  $$$$$$/ |  $$$$/|  $$$$$$$  |  $$$$/|  $$$$$$$
     \______/   \___/   \_______/   \___/   \______*/

    public virtual void OnUpdate() {}

    public virtual bool OnBack() { return true; } //Return true to close menu


     /*$$$$$$$                            /$$
    |__  $$__/                           | $$
       | $$  /$$$$$$   /$$$$$$   /$$$$$$ | $$  /$$$$$$
       | $$ /$$__  $$ /$$__  $$ /$$__  $$| $$ /$$__  $$
       | $$| $$  \ $$| $$  \ $$| $$  \ $$| $$| $$$$$$$$
       | $$| $$  | $$| $$  | $$| $$  | $$| $$| $$_____/
       | $$|  $$$$$$/|  $$$$$$$|  $$$$$$$| $$|  $$$$$$$
       |__/ \______/  \____  $$ \____  $$|__/ \_______/
                      /$$  \ $$ /$$  \ $$
                     |  $$$$$$/|  $$$$$$/
                      \______/  \_____*/

    public void Open(MenuManager menuManager, int order, object args = null) {
        //Init
        MenuManager = menuManager;
        Canvas.sortingOrder = order;
        Canvas.worldCamera = menuManager.MainCanvas.worldCamera;

        //Open
        State = MenuState.Open;
        OnOpen(args);
    }

    public void Close() {
        State = MenuState.Closed;
        OnClose();
    }

    public void CloseFromUI() {
        //Used to close the menu from UI components like a button
        MenuManager.CloseLast();
    }

    protected virtual void OnOpen(object args = null) {
        gameObject.SetActive(true);
    }

    protected virtual void OnClose() {
        gameObject.SetActive(false);
    }


      /*$$$$$
     /$$__  $$
    | $$  \__/  /$$$$$$  /$$    /$$ /$$$$$$   /$$$$$$
    | $$       /$$__  $$|  $$  /$$//$$__  $$ /$$__  $$
    | $$      | $$  \ $$ \  $$/$$/| $$$$$$$$| $$  \__/
    | $$    $$| $$  | $$  \  $$$/ | $$_____/| $$
    |  $$$$$$/|  $$$$$$/   \  $/  |  $$$$$$$| $$
     \______/  \______/     \_/    \_______/|_*/

    public void Cover() {
        State = MenuState.Covered;
        OnCovered();
    }

    public void Uncover() {
        State = MenuState.Open;
        OnUncovered();
    }

    protected virtual void OnCovered() {
        gameObject.SetActive(false);
    }

    protected virtual void OnUncovered() {
        gameObject.SetActive(true);
    }


      /*$$$$$    /$$     /$$
     /$$__  $$  | $$    | $$
    | $$  \ $$ /$$$$$$  | $$$$$$$   /$$$$$$   /$$$$$$
    | $$  | $$|_  $$_/  | $$__  $$ /$$__  $$ /$$__  $$
    | $$  | $$  | $$    | $$  \ $$| $$$$$$$$| $$  \__/
    | $$  | $$  | $$ /$$| $$  | $$| $$_____/| $$
    |  $$$$$$/  |  $$$$/| $$  | $$|  $$$$$$$| $$
     \______/    \___/  |__/  |__/ \_______/|_*/

    public static GameObject GetPrefabFromName(string name) {
        return Resources.Load<GameObject>("Prefabs/UI/Menus/" + name);
    }

}

