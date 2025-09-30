using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using Botpa;
using TMPro;
using UnityEngine.Localization;

public class InventoryMenu : Menu {

    //Prefab
    public override string Name => "Inventory";

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference inventoryAction;

    //Items
    //[Header("Items")]
    //[SerializeField] private InventoryItem[] items = new InventoryItem[9];


      /*$$$$$   /$$                 /$$
     /$$__  $$ | $$                | $$
    | $$  \__//$$$$$$    /$$$$$$  /$$$$$$    /$$$$$$
    |  $$$$$$|_  $$_/   |____  $$|_  $$_/   /$$__  $$
     \____  $$ | $$      /$$$$$$$  | $$    | $$$$$$$$
     /$$  \ $$ | $$ /$$ /$$__  $$  | $$ /$$| $$_____/
    |  $$$$$$/ |  $$$$/|  $$$$$$$  |  $$$$/|  $$$$$$$
     \______/   \___/   \_______/   \___/   \______*/

    public override void OnUpdate() {
        //Close menu
        if (inventoryAction.Triggered()) MenuManager.CloseLast();
    }


      /*$$$$$              /$$     /$$
     /$$__  $$            | $$    |__/
    | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
    | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
    | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
    | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
    | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
    |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    private void UpdateItems() {
        //Update / Reset items
        /*for (int i = 0; i < items.Length; i++) {
            //Check if item or empty
            if (i < Inventory.ItemsCount) {
                //Item -> Load it
                items[i].Load(Inventory.GetItem(i), Inventory.GetItemAmount(i));
            } else {
                //Empty -> Load nothing (reset)
                items[i].Load();
            }
        }*/
    }

    private void OnItemChanged(Item item, int amount) {
        UpdateItems();
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

        //Update items
        UpdateItems();

        //Add item change event
        //Inventory.AddOnItemChanged(OnItemChanged);

        //Pause game
        Game.Pause(this);
    }

    protected override void OnClose() {
        base.OnClose();

        //Remove item change event
        //Inventory.RemoveOnItemChanged(OnItemChanged);

        //Unpause game
        Game.Unpause(this);
    }

}
