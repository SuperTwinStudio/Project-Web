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
    [Header("Components")]
    [SerializeField] private Selectable _defaultSelected;

    private Loadout Loadout => Player.Loadout;

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference inventoryAction;

    //Shop
    [Header("Shop")]
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private LocalizedString goldLocale;

    //Character
    [Header("Character")]
    [SerializeField] private CharacterUpgrade characterGramaje;
    [SerializeField] private CharacterUpgrade characterRugosidad;

    //Weapon
    [Header("Weapon")]
    [SerializeField] private List<ShopWeaponTab> weaponTabs;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private WeaponUpgrade primaryAttack;
    [SerializeField] private WeaponUpgrade secondaryAttack;
    [SerializeField] private WeaponUpgrade passiveAttack;

    private Weapon selectedWeapon;


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
        //Update gold
        goldText.SetText($"{goldLocale.GetLocalizedString()} {Loadout.Gold}G");
    }

    //Character
    private void UpdateCharacterUI() {
        //Update upgrades
        characterGramaje.UpdateUI(Player.GramajeUpgrade, Loadout.Gold);
        characterRugosidad.UpdateUI(Player.RugosidadUpgrade, Loadout.Gold);
    }

    public void UpgradeCharacter(int type) {
        //Try to upgrade player
        if (!Player.TryUpgrade((PlayerUpgrade) type)) {
            _defaultSelected.Select();
            return; 
        }

        //Success -> Update UI
        UpdateMainUI();
        UpdateCharacterUI();
    }

    [Serializable]
    private class CharacterUpgrade {

        [SerializeField] private TMP_Text nameText;
        [SerializeField] private LocalizedString nameLocale;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private LocalizedString descriptionLocale;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;

        public void UpdateUI(Upgrade upgrade, int gold) {
            //Name & description
            nameText.SetText($"{nameLocale.GetLocalizedString()} - LvL {(upgrade.CanUpgrade ? upgrade.Level : "MAX")}");
            descriptionText.SetText(descriptionLocale.GetLocalizedString());

            //Upgrade button
            button.interactable = upgrade.CanUpgrade && gold >= upgrade.Cost;
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
        primaryAttack.UpdateUI(selectedWeapon, WeaponAction.Primary, Loadout.Gold);
        secondaryAttack.UpdateUI(selectedWeapon, WeaponAction.Secondary, Loadout.Gold);
        passiveAttack.UpdateUI(selectedWeapon, WeaponAction.Passive, Loadout.Gold);
    }

    public void SelectWeaponTab(Item item) {
        //Select weapon
        selectedWeapon = Loadout.GetWeapon(item);
        Loadout.SelectWeapon(item);

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
    private class WeaponUpgrade {

        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private LocalizedString nameLocale;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;

        public void UpdateUI(Weapon weapon, WeaponAction action, int gold) {
            //Get upgrade
            Upgrade upgrade = weapon.GetUpgrade(action);
        
            //Icon
            icon.sprite = action switch {
                WeaponAction.Primary => weapon.PrimaryIcon,
                WeaponAction.Secondary => weapon.SecondaryIcon,
                _ => weapon.PassiveIcon
            };

            //Name & description
            nameText.SetText($"{nameLocale.GetLocalizedString()} - LvL {(upgrade.CanUpgrade ? upgrade.Level : "MAX")}");
            descriptionText.SetText(action switch { 
                WeaponAction.Primary => weapon.PrimaryDescription, 
                WeaponAction.Secondary => weapon.SecondaryDescription, 
                _ => weapon.PassiveDescription, 
            });

            //Upgrade button
            button.interactable = upgrade.CanUpgrade && gold >= upgrade.Cost;
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

        //Save game
        Game.Current.SaveGame();

        //Stop camera cinematic
        Level.CameraController.ExitCutscene();
    }

}

public class ShopArgs {
    
    public Transform positionTarget;
    public Transform viewTarget;

}