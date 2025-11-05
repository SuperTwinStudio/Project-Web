using UnityEngine;

public class ShopTrigger : MonoBehaviour {

    //Components
    private MenuManager MenuManager => Game.Current.MenuManager;

    //Camera
    [Header("Camera")]
    [SerializeField] private Transform positionTarget;
    [SerializeField] private Transform viewTarget;


    //State
    private void OnTriggerEnter(Collider other) {
        //Check if player
        if (!other.CompareTag("Player")) return;

        //Open shop
        MenuManager.Open(MenusList.Shop, new ShopArgs() {
            positionTarget = positionTarget,
            viewTarget = viewTarget
        });
    }

}
