using UnityEngine;

public class ShopTrigger : MonoBehaviour {

    //Components
    private MenuManager MenuManager => Game.Current.MenuManager;

    //Camera
    [Header("Camera")]
    [SerializeField] private Transform viewTarget;
    [SerializeField] private Transform cameraTarget;


    //State
    private void OnTriggerEnter(Collider other) {
        //Check if player
        if (!other.CompareTag("Player")) return;

        //Open shop
        MenuManager.Open(MenusList.Shop, new ShopArgs() {
            viewTarget = viewTarget,
            cameraTarget = cameraTarget
        });
    }

}
