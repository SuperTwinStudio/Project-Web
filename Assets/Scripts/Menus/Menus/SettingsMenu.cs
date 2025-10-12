using UnityEngine;

public class SettingsMenu : Menu {
    
    //Prefab
    public override string Name => MenusList.Settings;

    //Components
    [Header("Components")]
    [SerializeField] private GameObject bgHome;
    [SerializeField] private GameObject bgGame;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject lobbyButton;


      /*$$$$$              /$$     /$$
     /$$__  $$            | $$    |__/
    | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
    | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
    | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
    | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
    | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
    |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    public void ReturnToHome() {
        Game.Current.LoadScene("Home");
    }

    public void ReturnToLobby() {
        //Show confirmation that player will lose all their items and half the money
        //For now it only returns to lobby and does not remove anything
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

        //Toggle return button
        homeButton.SetActive(gameObject.scene.name == "Lobby");
        lobbyButton.SetActive(gameObject.scene.name == "Dungeon");

        //Toggle backgrounds
        bgHome.SetActive(gameObject.scene.name == "Home");
        bgGame.SetActive(!bgHome.activeSelf);

        //Pause game
        Game.Pause(this);
    }

    protected override void OnClose() {
        base.OnClose();

        //Unpause game
        Game.Unpause(this);
    }

}
