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
        for (int i = room.Enemies.Count - 1; i >= 0 ; i--) { //Use reverse fori in case an enemy (miso beast) spawns more enemies inside the loop
            //Notify player entered the room
            room.Enemies[i].NotifyPlayerEnteredRoom();
        }
    }

}
