using UnityEngine;

public class Level : MonoBehaviour {

    //Game
    private Game _game;

    public Game Game {
        get {
            if (!_game) {
                _game = FindFirstObjectByType<Game>();
                if (!_game) Debug.LogError("Couldn't access Game from Level");
            }
            return _game;
        }
        private set => _game = value;
    }

    //Components
    [Header("Components")]
    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Player _player;

    public MenuManager MenuManager => _menuManager;
    public CameraController CameraController => _cameraController;
    public Player Player => _player;

    //Level
    [Header("Level")]
    [SerializeField] private bool _isLobby = false;
    [SerializeField] private bool _isHandmade = true;
    [SerializeField] private LevelDefinition _levelDef;

    public bool IsLobby => _isLobby;
    public bool IsHandmade => _isHandmade;


    //State
    private void Awake() {
        //Assign player to camera follow point
        CameraController.Follow = Player.transform;
    }

    private void Start()
    {
        if(!_isLobby && !_isHandmade) InitializeLevel();
    }

    private void InitializeLevel()
    {
        LevelGenerator generator = new LevelGenerator();
        generator.GenerateLevel(_levelDef);

        // Allow player interaction when level has generated
        Player.gameObject.SetActive(true);
        CameraController.gameObject.SetActive(true);
    }

    //Scene
    public void EnterDungeon() {
        Game.LoadScene("AlexDungeon");
    }

    public void ExitToLobby() {
        Game.LoadScene("AlexLobby");
    }

}
