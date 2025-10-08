using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopMenu : Menu {
  
    //Prefab
    public override string Name => MenusList.Shop;

    //Components
    private Loadout Loadout => Player.Loadout;

    //Shop
    [Header("Shop")]
    [SerializeField] private TMP_Text moneyText;

    //Class
    [Header("Class")]
    [SerializeField] private TMP_Text primaryUpgradeText;
    [SerializeField] private TMP_Text primaryCostText;
    [SerializeField] private TMP_Text secondaryUpgradeText;
    [SerializeField] private TMP_Text secondaryCostText;
    [SerializeField] private TMP_Text passiveUpgradeText;
    [SerializeField] private TMP_Text passiveCostText;


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
        moneyText.SetText($"Money: {Loadout.Money}G");

        //Update class tab
        UpdateClassUI();
    }

    //Weapons
    private void UpdateClassUI() {
        //
        primaryUpgradeText.SetText($"Upgrade to tier {Loadout.CurrentWeapon.PrimaryTier + 1}");
        primaryCostText.SetText($"{Loadout.CurrentWeapon.PrimaryUpgradeCost}G");

        //
        secondaryUpgradeText.SetText($"Upgrade to tier {Loadout.CurrentWeapon.SecondaryTier + 1}");
        secondaryCostText.SetText($"{Loadout.CurrentWeapon.SecondaryUpgradeCost}G");

        //
        passiveUpgradeText.SetText($"Upgrade to tier {Loadout.CurrentWeapon.PassiveTier + 1}");
        passiveCostText.SetText($"{Loadout.CurrentWeapon.PassiveUpgradeCost}G");
    }

    public void UpgradeWeapon(int type) {
        //Try to upgrade weapon
        if (Loadout.CurrentWeapon.Upgrade((WeaponType) type)) UpdateClassUI();
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
