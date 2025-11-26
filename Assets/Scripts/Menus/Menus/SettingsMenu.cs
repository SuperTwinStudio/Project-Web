using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : Menu {
    
    //Prefab
    public override string Name => MenusList.Settings;

    //Components
    [Header("Components")]
    [SerializeField] private List<Selectable> defaultSelectables;
    [SerializeField] private GameObject backgroundGame;
    [SerializeField] private GameObject backgroundHome;
    [SerializeField] private GameObject buttonsHome;
    [SerializeField] private GameObject buttonsLobby;
    [SerializeField] private GameObject buttonsDungeon;


      /*$$$$$              /$$     /$$
     /$$__  $$            | $$    |__/
    | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
    | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
    | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
    | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
    | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
    |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    private void SelectDefault() {
        foreach (var selectable in defaultSelectables) {
            if (!selectable.gameObject.activeInHierarchy) continue;
            selectable.Select();
            return;
        }
    }

    public void ReturnToHome() {
        //Show confirmation that player will lose all their items
        //For now it clears inventory directly
        Player.Loadout.ClearInventory();

        //Return to home
        Game.Current.LoadScene("Home");
    }

    public void ReturnToLobby() {
        //Show confirmation that player will lose all their items (for now it clears inventory directly)
        Player.Loadout.ClearInventory();

        //Return to lobby
        Game.Current.LoadScene("Lobby");
    }

    public void OpenTutorial() {
        //Open tutorial menu
        MenuManager.Open(MenusList.Tutorial);
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

        //Not playing
        if (!Application.isPlaying) return;

        //Get scene name
        string scene = SceneManager.GetActiveScene().name;

        //Toggle backgrounds
        backgroundHome.SetActive(scene == "Home");
        backgroundGame.SetActive(scene != "Home");

        //Toggle buttons
        switch (scene) {
            //Castle (lobby)
            case "Lobby":
                buttonsHome.SetActive(false);
                buttonsLobby.SetActive(true);
                buttonsDungeon.SetActive(false);
                break;
            //Dungeon
            case "Dungeon":
                buttonsHome.SetActive(false);
                buttonsLobby.SetActive(false);
                buttonsDungeon.SetActive(true);
                break;
            //Other (home)
            default:
                buttonsHome.SetActive(true);
                buttonsLobby.SetActive(false);
                buttonsDungeon.SetActive(false);
                break;
        }

        //Select default button (for controller navigation)
        SelectDefault();

        //Pause game
        Game.Pause(this);
    }

    protected override void OnClose() {
        base.OnClose();

        //Not playing
        if (!Application.isPlaying) return;

        //Unpause game
        Game.Unpause(this);
    }


      /*$$$$$
     /$$__  $$
    | $$  \__/  /$$$$$$  /$$    /$$ /$$$$$$   /$$$$$$
    | $$       /$$__  $$|  $$  /$$//$$__  $$ /$$__  $$
    | $$      | $$  \ $$ \  $$/$$/| $$$$$$$$| $$  \__/
    | $$    $$| $$  | $$  \  $$$/ | $$_____/| $$
    |  $$$$$$/|  $$$$$$/   \  $/  |  $$$$$$$| $$
     \______/  \______/     \_/    \_______/|_*/

    protected override void OnUncovered() {
        base.OnUncovered();

        //Select default button (for controller navigation)
        SelectDefault();
    }

}
