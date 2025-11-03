using System;
using System.Collections.Generic;
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

    //Info
    public static bool InGame { get; private set; }

    //Pause & Cursor
    private static readonly List<object> pause = new();
    private static readonly List<object> cursor = new();

    public static bool IsPaused { get; private set; } = false;

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
        bool hide = cursor.Count > 0;
        Cursor.visible = !hide;
        Cursor.lockState = hide ? CursorLockMode.Locked : CursorLockMode.None;
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
    public void LoadScene(string name) {
        //Save game so new scene keeps info
        if (InGame) save = OnSave();

        //Load scene
        SceneManager.LoadScene(name);
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
