using UnityEngine;

public class ShopTrigger : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private MeshRenderer meshRenderer;

    private CameraController cameraController;
    private Player player;

    private MenuManager MenuManager => Game.Current.MenuManager;

    //Camera
    [Header("Camera")]
    [SerializeField] private Transform positionTarget;
    [SerializeField] private Transform viewTarget;


    //State
    private void Start() {
        cameraController = Game.Current.Level.CameraController;
        player = Game.Current.Level.Player;
    }

    private void OnTriggerEnter(Collider other) {
        //Check if player
        if (!other.CompareTag("Player")) return;

        //Hide
        meshRenderer.enabled = false;

        //Enter camera cutscene
        cameraController.EnterCutscene(positionTarget, viewTarget);

        //Rotate player
        player.LookInDirection(Vector3.back);

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
