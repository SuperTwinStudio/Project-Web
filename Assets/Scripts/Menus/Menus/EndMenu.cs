using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class EndMenu : Menu {

    //Prefab
    public override string Name => MenusList.End;

    //Components
    [Header("Components")]
    [SerializeField] private Selectable defaultSelectable;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private LocalizedString infoDefaultLocale;
    [SerializeField] private LocalizedString infoWeaponLocale;

    private Loadout Loadout => Player.Loadout;


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
        defaultSelectable.Select();

        //Get non unlocked weapon items
        List<Item> items = new();
        foreach (var weapon in Loadout.Weapons) {
            //Get item
            Item item = weapon.Item;

            //Check if is already unlocked
            if (Loadout.Unlocked.Contains(item)) continue;

            //Not unlocked -> Add to list
            items.Add(weapon.Item);
        }

        //Check if a weapon can be unlocked
        if (items.Count == 0) {
            //Update text
            infoText.SetText(
                infoDefaultLocale.GetLocalizedString()
                    .Replace("<gold>", $"{Loadout.Gold}G")
                    .Replace("<treasures>", $"{Loadout.InventoryValue}G")
            );
        } else {
            //Unlock weapon
            Item item = items[Random.Range(0, items.Count)];
            Loadout.UnlockWeapon(item);

            //Update text
            infoText.SetText(
                infoWeaponLocale.GetLocalizedString()
                    .Replace("<weapon>", item.Name)
                    .Replace("<gold>", $"{Loadout.Gold}G")
                    .Replace("<treasures>", $"{Loadout.InventoryValue}G")
            );
        }

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