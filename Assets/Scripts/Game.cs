using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour, ISavable {

    //Singleton
    public static Game Current { get; private set; }

    //Components
    [Header("Components")]
    [SerializeField] private MenuManager _menuManager;

    private Level _level;

    public Level Level {
        get {
            if (!_level) {
                _level = FindFirstObjectByType<Level>();
                if (!_level) Debug.LogWarning("Couldn't access Level from Game");
            }
            return _level;
        }
        private set => _level = value;
    }

    public MenuManager MenuManager => _menuManager;

    //Pause & Cursor
    private static readonly List<object> pause = new();
    private static readonly List<object> cursor = new();

    public static bool IsPaused { get; private set; } = false;
    public static bool HasCursor { get; private set; } = true;

    //Info
    private static event Action<bool> OnLoadingChanged;

    public static bool InGame { get; private set; } = true;
    public static bool IsLoading { get; private set; } = false;

    public static bool IsPlaying => !IsPaused && !IsLoading;

    //Saving
    private string save = "{}";


    //State
    private void Awake() {
        //Singleton 🤓
        if (Current) {
            //Another game exists -> Destroy this one
            Current.OnNewGame(this);
            gameObject.SetActive(false);
            DestroyImmediate(gameObject);
            return;
        }

        //None exists -> Keep this one as singleton
        Current = this;
        OnNewGame(this);
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        //Init audio preferences
        Preferences.InitAudio();
    }

    private void OnNewGame(Game newGame) {
        //Unpause game & show cursor
        Unpause();
        UnhideCursor();

        //Check for a level
        InGame = Level;

        //Load game state
        if (InGame) OnLoad(save);

        //Init menus with new game menus
        MenuManager.Init(newGame.MenuManager);
    }

    //Pause & Cursor
    private static void UpdateIsPaused() {
        IsPaused = pause.Count > 0;
        Time.timeScale = IsPaused ? 0 : 1;
    }

    private static void UpdateCursorVisibility() {
        HasCursor = cursor.Count == 0;
        Cursor.visible = HasCursor;
        Cursor.lockState = HasCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private static void Unpause() {
        pause.Clear();
        UpdateIsPaused();
    }

    private static void UnhideCursor() {
        cursor.Clear();
        UpdateCursorVisibility();
    }

    public static void Pause(object obj) {
        if (!pause.Contains(obj)) pause.Add(obj);
        UpdateIsPaused();
    }

    public static void Unpause(object obj) {
        pause.Remove(obj);
        UpdateIsPaused();
    }

    public static void HideCursor(object obj) {
        if (!cursor.Contains(obj)) cursor.Add(obj);
        UpdateCursorVisibility();
    }

    public static void UnhideCursor(object obj) {
        cursor.Remove(obj);
        UpdateCursorVisibility();
    }

    //Scenes
    private IEnumerator LoadSceneCoroutine(string name) {
        //Get transition info
        (string triggerIn, string triggerOut, float duration) = MenuManager.GetTransitionInfo(MenuTransition.Circle);

        //Animate out
        MenuManager.MenuTransitions.SetTrigger(triggerOut);
        yield return new WaitForSecondsRealtime(duration);

        //Load scene
        SceneManager.LoadScene(name);
        yield return new WaitForNextFrameUnit();

        //Animate in
        MenuManager.MenuTransitions.SetTrigger(triggerIn);
        yield return new WaitForSecondsRealtime(duration);

        //Finish loading
        IsLoading = false;
        OnLoadingChanged?.Invoke(IsLoading);
    }

    public void LoadScene(string name) {
        //Already loading
        if (IsLoading) return;

        //Save game so new scene keeps info
        if (InGame) save = OnSave();

        //Load scene
        IsLoading = true;
        OnLoadingChanged?.Invoke(IsLoading);
        StartCoroutine(LoadSceneCoroutine(name));
    }

    public static void AddOnLoadingChanged(Action<bool> action) {
        OnLoadingChanged += action;
    }

    public static void RemoveOnLoadingChanged(Action<bool> action) {
        OnLoadingChanged -= action;
    }

    //Saving
    public string OnSave() {
        return JsonUtility.ToJson(new GameSave() {
            //Player
            player = Level.Player.OnSave()
        });
    }

    public void OnLoad(string saveJson) {
        //Parse save
        var save = JsonUtility.FromJson<GameSave>(saveJson);

        //Load player
        Level.Player.OnLoad(save.player);
    }

    [Serializable]
    private class GameSave {

        //Player
        public string player = "{}";

    }

}
