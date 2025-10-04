using UnityEngine;

public class ShopTrigger : MonoBehaviour {

    //Components
    private MenuManager MenuManager => Game.Current.Level.MenuManager;


    //State
    private void OnTriggerEnter(Collider other) {
        //Check if player
        if (!other.CompareTag("Player")) return;

        //Open shop
        MenuManager.Open(MenusList.Shop);
    }

}
