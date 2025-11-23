using UnityEngine;

public class ShopTrigger : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private MeshRenderer meshRenderer;

    private MenuManager MenuManager => Game.Current.MenuManager;

    //Camera
    [Header("Camera")]
    [SerializeField] private Transform positionTarget;
    [SerializeField] private Transform viewTarget;


    //State
    private void OnTriggerEnter(Collider other) {
        //Check if player
        if (!other.CompareTag("Player")) return;

        //Hide
        meshRenderer.enabled = false;

        //Enter camera cutscene
        Game.Current.Level.CameraController.EnterCutscene(positionTarget, viewTarget);

        //Open shop
        MenuManager.Open(MenusList.Shop);
    }

    private void OnTriggerExit(Collider other) {
        //Check if player
        if (!other.CompareTag("Player")) return;

        //Show
        meshRenderer.enabled = true;
    }

}
