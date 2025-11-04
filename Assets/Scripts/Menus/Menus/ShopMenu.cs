using System;
using System.Collections.Generic;
using System.Linq;
using Botpa;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
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

    //Character
    [Header("Character")]
    [SerializeField] private CharacterUpgrade characterGramaje;
    [SerializeField] private CharacterUpgrade characterRugosidad;

    //Weapon
    [Header("Weapon")]
    [SerializeField] private List<WeaponTab> weaponTabs;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private WeaponUpgrade primaryAttack;
    [SerializeField] private WeaponUpgrade secondaryAttack;
    [SerializeField] private WeaponUpgrade passiveAttack;

    private Weapon selectedWeapon;

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

    //UI
    private void UpdateUI() {
        //Update main
        UpdateMainUI();

        //Update character tab
        UpdateCharacterUI();

        //Update weapons tab
        UpdateWeaponUI();
    }

    //Main
    private void UpdateMainUI() {
        //Update money
        moneyText.SetText($"{Util.Localize("indicator_money")} {Loadout.Money}G");
    }

    //Character
    private void UpdateCharacterUI() {
        //Update upgrades
        characterGramaje.UpdateUI(Player.GramajeUpgrade, Loadout.Money);
        characterRugosidad.UpdateUI(Player.RugosidadUpgrade, Loadout.Money);
    }

    public void UpgradeCharacter(int type) {
        //Try to upgrade player
        if (!Player.TryUpgrade((PlayerUpgrade) type)) 
        {
            _defaultSelected.Select();
            return; 
        }

        //Success -> Update UI
        UpdateMainUI();
        UpdateCharacterUI();
    }

    [Serializable]
    private class CharacterUpgrade {

        public TMP_Text nameText;
        public LocalizedString nameLocale;
        public TMP_Text descriptionText;
        public Button button;
        public TMP_Text buttonText;

        public void UpdateUI(Upgrade upgrade, int money) {
            //Name & description
            nameText.SetText($"{nameLocale.GetLocalizedString()} - LvL {(upgrade.CanUpgrade ? upgrade.Level : "MAX")}");
            //Update description text

            //Upgrade button
            button.interactable = upgrade.CanUpgrade && money >= upgrade.Cost;
            buttonText.SetText(upgrade.CanUpgrade ? $"{Util.Localize("shop_weapon_upgrade")}\n{upgrade.Cost}G" : "MAX");
        }

    }

    //Weapon
    private void UpdateWeaponUI() {
        //Prelocalize upgrade text & get current weapon
        if (!selectedWeapon) selectedWeapon = Loadout.CurrentWeapon;

        //Update tabs
        foreach (var tab in weaponTabs) tab.UpdateUI(Loadout);

        //Update upgrades
        weaponNameText.SetText(selectedWeapon.Item.Name);
        primaryAttack.UpdateUI(selectedWeapon, WeaponAction.Primary, Loadout.Money);
        secondaryAttack.UpdateUI(selectedWeapon, WeaponAction.Secondary, Loadout.Money);
        passiveAttack.UpdateUI(selectedWeapon, WeaponAction.Passive, Loadout.Money);
    }

    public void SelectWeaponTab(Item item) {
        //Select weapon
        selectedWeapon = Loadout.GetWeapon(item);

        //Update UI
        UpdateWeaponUI();
    }

    public void UpgradeWeapon(int type) {
        //Try to upgrade weapon
        if (!selectedWeapon.TryUpgrade((WeaponAction) type)) return;

        //Success -> Update UI
        UpdateMainUI();
        UpdateWeaponUI();
    }

    [Serializable]
    private class WeaponTab {

        public Item item;
        public Button button;

        public void UpdateUI(Loadout loadout) {
            button.interactable = loadout.Unlocked.Contains(item);
        }

    }

    [Serializable]
    private class WeaponUpgrade {

        public Image icon;
        public TMP_Text nameText;
        public LocalizedString nameLocale;
        public TMP_Text descriptionText;
        public Button button;
        public TMP_Text buttonText;

        public void UpdateUI(Weapon weapon, WeaponAction attack, int money) {
            //Get upgrade
            Upgrade upgrade = weapon.GetUpgrade(attack);
        
            //Icon
            icon.sprite = attack switch {
                WeaponAction.Primary => weapon.PrimaryIcon,
                WeaponAction.Secondary => weapon.SecondaryIcon,
                _ => weapon.PassiveIcon
            };

            //Name & description
            nameText.SetText($"{nameLocale.GetLocalizedString()} - LvL {(upgrade.CanUpgrade ? upgrade.Level : "MAX")}");
            //Update description text

            //Upgrade button
            button.interactable = upgrade.CanUpgrade && money >= upgrade.Cost;
            buttonText.SetText(upgrade.CanUpgrade ? $"{Util.Localize("shop_weapon_upgrade")}\n{upgrade.Cost}G" : "MAX");
        }

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

        //Select current weapon
        selectedWeapon = Loadout.CurrentWeapon;

        //Select default button (for controller navigation)
        _defaultSelected.Select();

        //Update UI
        UpdateUI();

        //Start camera cinematic
        if (args != null) {
            ShopArgs shopArgs = (ShopArgs) args;
            Level.CameraController.EnterCutscene(shopArgs.positionTarget, shopArgs.viewTarget);
        }
    }

    protected override void OnClose() {
        base.OnClose();

        //Not playing
        if (!Application.isPlaying) return;

        //Stop camera cinematic
        Level.CameraController.ExitCutscene();
    }

}

public class ShopArgs {
    
    public Transform positionTarget;
    public Transform viewTarget;

}