using UnityEngine.InputSystem;
using UnityEngine;
using Botpa;
using TMPro;
using UnityEngine.UI;

public class InventoryMenu : Menu {

    //Prefab
    public override string Name => MenusList.Inventory;

    //Components
    private Loadout Loadout => Player.Loadout;

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference inventoryAction;

    //Inventory
    [Header("Inventory")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private GameObject emptyMessage;
    [SerializeField] private RectTransform itemsGrid;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private GameObject itemPrefab;

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

    private void UpdateUI() {
        //Update money & inventory value
        moneyText.SetText($"{Util.Localize("indicator_money")} {Loadout.Money}G");
        valueText.SetText($"{Util.Localize("indicator_value")} {Loadout.InventoryValue}G");

        //Clear old items
        Util.DestroyChildren(itemsGrid);

        //Select default button (for controller navigation)
        _defaultSelected.Select();

        //Add new items
        foreach (var pair in Loadout.Inventory) Instantiate(itemPrefab, itemsGrid).GetComponent<InventoryItem>().Init(pair.Key, pair.Value);

        //Toggle empty message
        emptyMessage.SetActive(Loadout.Inventory.Count <= 0);
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

        //Update items
        UpdateUI();

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
