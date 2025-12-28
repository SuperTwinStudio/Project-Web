using UnityEngine;

public class TestingRoom : MonoBehaviour {

    [SerializeField] private Room room;

    private void Awake() {
        //Assing enemies to room
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None)) {
            //Not active
            if (!enemy.gameObject.activeInHierarchy) return;

            //Init enemy in room
            room.InitializeEnemy(enemy);
        }
    }

    private void Start() {
        //Assing enemies to room
        foreach (var enemy in room.Enemies) {
            //Notify player entered the room
            enemy.NotifyPlayerEnteredRoom();
        }
    }

}
