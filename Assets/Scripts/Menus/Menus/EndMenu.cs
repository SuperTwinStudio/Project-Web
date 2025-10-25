using UnityEngine;
using UnityEngine.UI;

public class EndMenu : Menu {

    //Prefab
    public override string Name => MenusList.End;

    [Header("Components")]
    [SerializeField] private Selectable _defaultSelected;

    /*$$$$$   /$$                 /$$
   /$$__  $$ | $$                | $$
  | $$  \__//$$$$$$    /$$$$$$  /$$$$$$    /$$$$$$
  |  $$$$$$|_  $$_/   |____  $$|_  $$_/   /$$__  $$
   \____  $$ | $$      /$$$$$$$  | $$    | $$$$$$$$
   /$$  \ $$ | $$ /$$ /$$__  $$  | $$ /$$| $$_____/
  |  $$$$$$/ |  $$$$/|  $$$$$$$  |  $$$$/|  $$$$$$$
   \______/   \___/   \_______/   \___/   \______*/

    public override bool OnBack() {
        return false; //Prevent close
    }


      /*$$$$$              /$$     /$$
     /$$__  $$            | $$    |__/
    | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
    | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
    | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
    | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
    | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
    |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    public void ReturnToCastle() {
        Game.Current.LoadScene("Lobby");
    }

    public void Continue() {
        Game.Current.LoadScene("Dungeon");
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

        //Select default button (for controller navigation)
        _defaultSelected.Select();

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

}