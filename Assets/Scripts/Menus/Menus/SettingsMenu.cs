using Botpa;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class SettingsMenu : Menu {
    
    //Prefab
    public override string Name => MenusList.Settings;

    //Components
    [Header("Components")]
    [SerializeField] private GameObject bgHome;
    [SerializeField] private GameObject bgGame;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject lobbyButton;
    [SerializeField] private Selectable _defaultSelected;

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference _pauseAction;


    /*$$$$$              /$$     /$$
   /$$__  $$            | $$    |__/
  | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
  | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
  | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
  | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
  | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
  |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    public void ReturnToHome() {
        //Show confirmation that player will lose all their items
        //For now it clears inventory directly
        Player.Loadout.ClearInventory();

        //Return to home
        Game.Current.LoadScene("Home");
    }

    public void ReturnToLobby() {
        //Show confirmation that player will lose all their items
        //For now it clears inventory directly
        Player.Loadout.ClearInventory();

        //Return to lobby
        Game.Current.LoadScene("Lobby");
    }


     /*$$$$$$$                            /$$
    |__  $$__/                           | $$
       | $$  /$$$$$$   /$$$$$$   /$$$$$$ | $$  /$$$$$$
       | $$ /$$__  $$ /$$__  $$ /$$__  $$| $$ /$$__  $$
       | $$| $$  \ $$| $$  \ $$| $$  \ $$| $$| $$$$$$$$
       | $$| $$  | $$| $$  | $$| $$  | $$| $$| $$_____/
       | $$|  $$$$$$/|  $$$$$$$|  $$$$$$$| $$|  $$$$$$$
       |__/ \______/  \____  $$ \____  $$|__/ \_______/
                      /$$  \ $$ /$$  \ $$
                     |  $$$$$$/|  $$$$$$/
                      \______/  \_____*/

    protected override void OnOpen(object args = null) {
        base.OnOpen();

        //Get scene name
        string scene = gameObject.scene.name;

        //Toggle return button
        homeButton.SetActive(scene == "Lobby");
        lobbyButton.SetActive(scene == "Dungeon");

        //Toggle backgrounds
        bgHome.SetActive(scene == "Home");
        bgGame.SetActive(scene != "Home");

        //Select default button (for controller navigation)
        _defaultSelected.Select();

        //Not playing
        if (!Application.isPlaying) return;

        _pauseAction.Disable();

        //Pause game
        Game.Pause(this);
    }

    protected override void OnClose() {
        base.OnClose();

        //Not playing
        if (!Application.isPlaying) return;

        _pauseAction.Enable();

        //Unpause game
        Game.Unpause(this);
    }

}
