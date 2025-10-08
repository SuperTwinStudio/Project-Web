using System;
using Botpa;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private UpgradeItem primaryAttack;
    [SerializeField] private UpgradeItem secondaryAttack;
    [SerializeField] private UpgradeItem passiveAttack;


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
        //Update main
        UpdateMainUI();

        //Update class tab
        UpdateClassUI();

        //Update player tab
        UpdatePlayerUI();
    }

    //Main
    private void UpdateMainUI() {
        //Update money
        moneyText.SetText($"{Util.Localize("indicator_money")} {Loadout.Money}G");
    }

    //Weapons
    private void UpdateClassUI() {
        //Prelocalize upgrade text & get current weapon
        string upgradeString = Util.Localize("shop_weapon_upgrade");
        Weapon weapon = Loadout.CurrentWeapon;

        //Update UI
        primaryAttack.UpdateUI(weapon.PrimaryIcon, Loadout.Money, weapon.PrimaryUpgradeCost, $"{upgradeString} {Loadout.CurrentWeapon.PrimaryLevel + 1}");
        secondaryAttack.UpdateUI(weapon.SecondaryIcon, Loadout.Money, weapon.SecondaryUpgradeCost, $"{upgradeString} {Loadout.CurrentWeapon.SecondaryLevel + 1}");
        passiveAttack.UpdateUI(weapon.PassiveIcon, Loadout.Money, weapon.PassiveUpgradeCost, $"{upgradeString} {Loadout.CurrentWeapon.PassiveLevel + 1}");
    }

    public void UpgradeWeapon(int type) {
        //Try to upgrade weapon
        if (Loadout.CurrentWeapon.Upgrade((WeaponAttack) type)) {
            UpdateMainUI();
            UpdateClassUI();
        }
    }

    [Serializable]
    private class UpgradeItem {

        public Image icon;
        public Button upgradeButton;
        public TMP_Text upgradeText;
        public TMP_Text upgradeCostText;

        public void UpdateUI(Sprite weaponIcon, int money, int cost, string level) {
            icon.sprite = weaponIcon;
            upgradeButton.interactable = money >= cost;
            upgradeText.SetText(level);
            upgradeCostText.SetText($"{cost}G");
        }

    }

    //Player
    private void UpdatePlayerUI() {}


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
