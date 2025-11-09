using Unity.AI.Navigation;
using UnityEngine;

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

    public CameraController CameraController => _cameraController;
    public Player Player => _player;

    //Level
    [Header("Level")]
    [SerializeField] private bool _isLobby = false;
    [SerializeField] private bool _isHandmade = true;
    [SerializeField] private NavMeshSurface _surface;

    public LevelDefinition Definition;

    public bool IsLobby => _isLobby;
    public bool IsHandmade => _isHandmade;
    public NavMeshSurface Surface => _surface;


    //State
    private void Start() {
        //Ignore lobby
        if (IsLobby) return;

        //Generate AI surface
        Surface.BuildNavMesh();

        //Init dungeon
        if (!IsHandmade) InitializeLevel();
    }

    private void InitializeLevel() {
        LevelGenerator generator = new();
        generator.GenerateLevel(Definition);

        //Allow player interaction when level has generated
        Player.gameObject.SetActive(true);
        CameraController.gameObject.SetActive(true);
    }

    //World
    public void UpdateWalkableSurface() {
        Surface.UpdateNavMesh(Surface.navMeshData);
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
