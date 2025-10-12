using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour {

    //Components
    private MenuManager MenuManager => Game.Current.MenuManager;

    [SerializeField] Transform viewTarget;
    [SerializeField] Transform cameraTarget;


    //State
    private void OnTriggerEnter(Collider other) {
        //Check if player
        if (!other.CompareTag("Player")) return;

        List<IMenuAction> actions = new List<IMenuAction>();

        actions.Add(new CloseAction(() => Game.Current.Level.CameraController.ExitCutScene()));

        //Open shop
        MenuManager.Open(MenusList.Shop, actions);

        Game.Current.Level.CameraController.EnterCutScene(viewTarget, cameraTarget);
    }

}
