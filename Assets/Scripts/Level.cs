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
    [SerializeField] private Player _player;

    public MenuManager MenuManager => _menuManager;
    public Player Player => _player;

}
