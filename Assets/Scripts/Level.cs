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
    [SerializeField] private bool _isLobby;

    public bool IsLobby => _isLobby;


    //State
    private void Awake() {
        //Assign player to camera follow point
        CameraController.Follow = Player.transform;
    }

    //Scene
    public void EnterDungeon() {
        Game.LoadScene("AlexDungeon");
    }

    public void ExitToLobby() {
        Game.LoadScene("AlexLobby");
    }

}
