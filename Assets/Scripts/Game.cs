using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    //Singleton
    public static Game Current { get; private set; }

    //Level
    private Level _level;

    public Level Level {
        get {
            if (!_level) {
                _level = FindFirstObjectByType<Level>();
                if (!_level) Debug.LogError("Couldn't access Level from Game");
            }
            return _level;
        }
        private set => _level = value;
    }

    //Pause & Cursor
    private static readonly List<object> pause = new();
    private static readonly List<object> cursor = new();
    
    public static bool IsPaused { get; private set; } = false;


    //State
    private void Awake() {
        //Singleton 🤓
        if (Current) {
            //Another game exists -> Destroy this one
            Current.OnNewGame(this);
            gameObject.SetActive(false);
            Destroy(gameObject);
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

        //Init menus with new game menus
        Level.MenuManager.Init(newGame.Level.MenuManager);
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

}
