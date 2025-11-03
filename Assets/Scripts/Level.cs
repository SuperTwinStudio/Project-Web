using Botpa;
using UnityEngine;
using UnityEngine.InputSystem;

public class Level : MonoBehaviour {

    //Game
    private Game _game;

    public Game Game {
        get {
            if (!_game) {
                _game = FindFirstObjectByType<Game>();
                if (!_game) Debug.LogWarning("Couldn't access Game from Level");
            }
            return _game;
        }
        private set => _game = value;
    }

    //Components
    [Header("Components")]
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Player _player;

    [Header("Input")]
    [SerializeField] private InputActionReference _pauseAction;

    public CameraController CameraController => _cameraController;
    public Player Player => _player;

    //Level
    [Header("Level")]
    [SerializeField] private bool _isLobby = false;
    [SerializeField] private bool _isHandmade = true;
    public LevelDefinition Definition;

    public bool IsLobby => _isLobby;
    public bool IsHandmade => _isHandmade;


    //State
    private void Awake() {
        CameraController.playerTransform = Player.transform;
    }

    private void Start() {
        if (!_isLobby && !_isHandmade) InitializeLevel();
    }

    private void Update() {
        if (_pauseAction.Triggered() && !Game.IsPaused) PauseLevel();
    }

    private void InitializeLevel() {
        LevelGenerator generator = new LevelGenerator();
        generator.GenerateLevel(Definition);

        // Allow player interaction when level has generated
        Player.gameObject.SetActive(true);
        CameraController.gameObject.SetActive(true);
    }

    //Scene
    public void EnterDungeon() {
        Game.LoadScene("Dungeon");
    }

    public void ExitToLobby() {
        Game.LoadScene("AlexLobby");
    }

    public void PauseLevel() {
        Game.Current.MenuManager.Open(MenusList.Settings);
    }

}
