using Botpa;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopMenu : Menu {
  
    //Prefab
    public override string Name => MenusList.Shop;

    //Components
    private Loadout Loadout => Player.Loadout;

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference inventoryAction;

    //Shop
    [Header("Shop")]
    [SerializeField] private TMP_Text moneyText;

    //Class
    [Header("Class")]
    [SerializeField] private Image primaryIcon;
    [SerializeField] private TMP_Text primaryUpgradeText;
    [SerializeField] private TMP_Text primaryCostText;
    [SerializeField] private Image secondaryIcon;
    [SerializeField] private TMP_Text secondaryUpgradeText;
    [SerializeField] private TMP_Text secondaryCostText;
    [SerializeField] private Image passiveIcon;
    [SerializeField] private TMP_Text passiveUpgradeText;
    [SerializeField] private TMP_Text passiveCostText;


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
        //Update money
        moneyText.SetText($"{Util.Localize("indicator_money")} {Loadout.Money}G");

        //Update class tab
        UpdateClassUI();
    }

    //Weapons
    private void UpdateClassUI() {
        string upgradeString = Util.Localize("shop_weapon_upgrade");

        //Primary
        primaryIcon.sprite = Loadout.CurrentWeapon.PrimaryIcon;
        primaryUpgradeText.SetText($"{upgradeString} {Loadout.CurrentWeapon.PrimaryLevel + 1}");
        primaryCostText.SetText($"{Loadout.CurrentWeapon.PrimaryUpgradeCost}G");

        //Secondary
        secondaryIcon.sprite = Loadout.CurrentWeapon.SecondaryIcon;
        secondaryUpgradeText.SetText($"{upgradeString} {Loadout.CurrentWeapon.SecondaryLevel + 1}");
        secondaryCostText.SetText($"{Loadout.CurrentWeapon.SecondaryUpgradeCost}G");

        //Passive
        passiveIcon.sprite = Loadout.CurrentWeapon.PassiveIcon;
        passiveUpgradeText.SetText($"{upgradeString} {Loadout.CurrentWeapon.PassiveLevel + 1}");
        passiveCostText.SetText($"{Loadout.CurrentWeapon.PassiveUpgradeCost}G");
    }

    public void UpgradeWeapon(int type) {
        //Try to upgrade weapon
        if (Loadout.CurrentWeapon.Upgrade((WeaponAttack) type)) UpdateClassUI();
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

        //Update UI
        UpdateUI();

        //Pause game
        Game.Pause(this);
    }

    protected override void OnClose() {
        base.OnClose();

        //Unpause game
        Game.Unpause(this);
    }

}
