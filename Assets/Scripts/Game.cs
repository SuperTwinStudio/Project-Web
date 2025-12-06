using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    //Cursor
    private static readonly List<object> cursorLock = new();

    public static bool HasCursor { get; private set; } = true;

    //Time (pause & slow)
    private static readonly List<object> pauseLock = new();
    private static readonly List<object> timeSlowLock = new();

    public static bool IsPaused { get; private set; } = false;
    public static bool IsTimeSlowed { get; private set; } = false;

    //Info
    private static event Action<bool> OnLoadingChanged;

    public static bool InGame { get; private set; } = true;
    public static bool IsLoading { get; private set; } = false;

    public static bool IsPlaying => !IsPaused && !IsLoading;

    //Saving
    private const int SAVE_VERSION = 3;

    private bool saveFileLoaded = false;
    private bool saveIsLoading = false;
    private bool saveAfterLoad = false;
    private string save = "{}";

    private string SavePath => $"{Application.persistentDataPath}/save.paper";

    public bool SaveExists => File.Exists(SavePath);


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
    }

    private void Start() {
        //Init audio preferences (AudioMixer can't be init in Awake)
        Preferences.Init();
    }

    private void OnNewGame(Game newGame) {
        //Reset cursor & time
        ResetHasCursor();
        ResetTime();

        //Check for a level
        InGame = Level;
    
        //Load game state
        if (InGame) LoadGame();

        //Init menus with new game menus
        MenuManager.Init(newGame.MenuManager);
    }

    //Cursor
    private static void UpdateHasCursor() {
        HasCursor = cursorLock.Count == 0;
        Cursor.visible = HasCursor;
        Cursor.lockState = HasCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private static void ResetHasCursor() {
        cursorLock.Clear();
        UpdateHasCursor();
    }

    public static void HideCursor(object obj) {
        if (!cursorLock.Contains(obj)) cursorLock.Add(obj);
        UpdateHasCursor();
    }

    public static void UnhideCursor(object obj) {
        cursorLock.Remove(obj);
        UpdateHasCursor();
    }

    //Time (pause & slow)
    private static void UpdateTime() {
        IsPaused = pauseLock.Count > 0;
        IsTimeSlowed = timeSlowLock.Count > 0;
        Time.timeScale = IsPaused ? 0 : IsTimeSlowed ? 0.8f : 1;
    }

    private static void ResetTime() {
        pauseLock.Clear();
        timeSlowLock.Clear();
        UpdateTime();
    }

    public static void Pause(object obj) {
        if (!pauseLock.Contains(obj)) pauseLock.Add(obj);
        UpdateTime();
    }

    public static void Unpause(object obj) {
        pauseLock.Remove(obj);
        UpdateTime();
    }

    public static void SlowTime(object obj) {
        if (!timeSlowLock.Contains(obj)) timeSlowLock.Add(obj);
        UpdateTime();
    }

    public static void UnslowTime(object obj) {
        timeSlowLock.Remove(obj);
        UpdateTime();
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
        yield return new WaitForNextFrameUnit(); //Wait again in case scenes take too long to load

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

        //Soft save game (without writing to disk) so new scene keeps info
        if (InGame) SaveGame(false);

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
    public void LoadGame() {
        //Check if file needs to be loaded
        if (!saveFileLoaded && SaveExists) {
            //Load file
            save = File.ReadAllText(SavePath);
            saveFileLoaded = true;
        }

        //Load game
        saveIsLoading = true;
        OnLoad(save);
        saveIsLoading = false;

        //Check if we need to save
        if (saveAfterLoad) SaveGame();
    }

    public void SaveGame(bool writeToDisk = true) {
        //Currently loading
        if (saveIsLoading) {
            saveAfterLoad = true;
            return;
        }

        //Update save
        save = OnSave();

        //Write save to file
        if (writeToDisk) File.WriteAllText(SavePath, save);
    }

    public string OnSave() {
        return JsonUtility.ToJson(new GameSave() {
            //Version
            version = SAVE_VERSION,
            //Level
            level = Level.OnSave(),
            //Player
            player = Level.Player.OnSave()
        });
    }

    public void OnLoad(string saveJson) {
        //Parse save
        var save = JsonUtility.FromJson<GameSave>(saveJson);

        //Load level
        Level.OnLoad(save.level);

        //Load player
        Level.Player.OnLoad(save.player);
    }

    [Serializable]
    private class GameSave {

        //Save version
        public int version = SAVE_VERSION;

        //Level
        public string level = "{}";

        //Player
        public string player = "{}";

    }

}
