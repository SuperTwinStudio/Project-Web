using System;
using Unity.AI.Navigation;
using UnityEngine;

public class Level : MonoBehaviour, ISavable {

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
    [SerializeField] private NavMeshSurface _surface;
    [SerializeField] private LevelDefinition _definition;
    [SerializeField] private Light _mainLight;

    public int Floor { get; private set; } = 0;

    public bool IsLobby => _isLobby;
    public NavMeshSurface Surface => _surface;
    public LevelDefinition Definition => _definition;


    //State
    private void Start() {
        //Ignore lobby
        if (IsLobby) return;

        //Generate AI surface
        Surface.BuildNavMesh();

        //Init dungeon
        if (!IsLobby) InitDungeon();
    }

    private void InitDungeon() {
        RenderSettings.ambientLight = Definition.LightColor;
        RenderSettings.customReflectionTexture = Definition.reflectionMap;

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

    //Saving
    public string OnSave() {
        return JsonUtility.ToJson(new LevelSave() {
            //Floor
            floor = Floor
        });
    }

    public void OnLoad(string saveJson) {
        //Parse save
        var save = JsonUtility.FromJson<LevelSave>(saveJson);

        //Load floor number (only happens on scene creation, so if not in lobby we are in a dungeon)
        if (IsLobby) {
            //In lobby -> Reset floor number
            Floor = 0;
            Game.SaveGame();
        } else {
            //In dungeon -> Increase previous level floor number by one
            Floor = save.floor + 1;
        }
    }

    [Serializable]
    private class LevelSave {

        //Floor
        public int floor = 0;

    }

}
