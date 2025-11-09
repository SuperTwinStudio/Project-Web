using Botpa;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UI;

public class GameMenu : Menu {

    //Prefab
    public override string Name => MenusList.Game;

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private InputActionReference inventoryAction;

    //Health
    [Header("Health")]
    [SerializeField] private TMP_Text healthText;

    //Weapon
    private Weapon currentWeapon;

    //Primary
    [Header("Primary")]
    [SerializeField] private Image primaryIcon;
    [SerializeField] private GameObject primaryValueBadge;
    [SerializeField] private TMP_Text primaryValueText;
    [SerializeField] private Image primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField] private Image secondaryIcon;
    [SerializeField] private GameObject secondaryValueBadge;
    [SerializeField] private TMP_Text secondaryValueText;
    [SerializeField] private Image secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField] private Image passiveIcon;
    [SerializeField] private GameObject passiveValueBadge;
    [SerializeField] private TMP_Text passiveValueText;
    [SerializeField] private Image passiveCooldown;

    //Reload
    [Header("Reload")]
    [SerializeField] private GameObject reloadIndicator;
    [SerializeField] private Slider reloadSlider;

    //Inventory Sold
    [Header("Inventory Sold")]
    [SerializeField] private Animator soldAnimator;
    [SerializeField] private TMP_Text soldText;
    [SerializeField] private LocalizedString soldLocale;

    //Item
    [Header("Passive Item")]
    [SerializeField] private Animator itemAnimator;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDesc;

    //Minimap
    [Header("Minimap")]
    [SerializeField] private GameObject minimapCamera;
    [SerializeField] private GameObject minimap;

      /*$$$$$   /$$                 /$$
     /$$__  $$ | $$                | $$
    | $$  \__//$$$$$$    /$$$$$$  /$$$$$$    /$$$$$$
    |  $$$$$$|_  $$_/   |____  $$|_  $$_/   /$$__  $$
     \____  $$ | $$      /$$$$$$$  | $$    | $$$$$$$$
     /$$  \ $$ | $$ /$$ /$$__  $$  | $$ /$$| $$_____/
    |  $$$$$$/ |  $$$$/|  $$$$$$$  |  $$$$/|  $$$$$$$
     \______/   \___/   \_______/   \___/   \______*/

    private void Update() {
        //Update icons cooldown
        if (currentWeapon) {
            primaryCooldown.fillAmount = currentWeapon.PrimaryCooldown;
            secondaryCooldown.fillAmount = currentWeapon.SecondaryCooldown;
            passiveCooldown.fillAmount = currentWeapon.PassiveCooldown;
        }
    }

    public override void OnUpdate() {
        //Open pause
        if (pauseAction.Triggered()) MenuManager.Open(MenusList.Settings);
        //Open inventory
        else if (inventoryAction.Triggered()) MenuManager.Open(MenusList.Inventory);

        //Move minimap camera
        Vector3 playerPos = Player.transform.position;
        playerPos.y = 100;
        minimapCamera.transform.position = playerPos;
    }

    public override bool OnBack() {
        return false;
    }


      /*$$$$$              /$$     /$$
     /$$__  $$            | $$    |__/
    | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
    | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
    | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
    | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
    | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
    |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

    //Health
    private void UpdateHealthIndicator(float health) {
        //Update health UI
        healthText.SetText($"{health}HP");
    }

    //Gold
    public void ShowInventorySold(int value) {
        //No value
        if (value <= 0) return;

        //Show animation
        soldText.SetText($"{soldLocale.GetLocalizedString()} {value}G");
        soldAnimator.SetTrigger("Show");
    }

    //Weapon
    private void OnWeaponChanged(Weapon oldWeapon, Weapon newWeapon) {
        //No weapon -> Ignore
        if (!newWeapon) return;

        //Update current weapon
        currentWeapon = newWeapon;

        //Update icons
        primaryIcon.sprite = newWeapon.PrimaryIcon;
        secondaryIcon.sprite = newWeapon.SecondaryIcon;
        passiveIcon.sprite = newWeapon.PassiveIcon;

        //Update cooldowns
        primaryCooldown.fillAmount = newWeapon.PrimaryCooldown;
        secondaryCooldown.fillAmount = newWeapon.SecondaryCooldown;
        passiveCooldown.fillAmount = newWeapon.PassiveCooldown;

        //Update reload
        reloadIndicator.SetActive(newWeapon.IsReloading);
        reloadSlider.value = newWeapon.ReloadValue;

        //Update values
        OnWeaponValueChanged(WeaponAction.Primary, newWeapon.PrimaryValue);
        OnWeaponValueChanged(WeaponAction.Secondary, newWeapon.SecondaryValue);
        OnWeaponValueChanged(WeaponAction.Passive, newWeapon.PassiveValue);
        if (oldWeapon) oldWeapon.RemoveOnValueChanged(OnWeaponValueChanged);
        if (newWeapon) newWeapon.AddOnValueChanged(OnWeaponValueChanged);
    }

    private void OnWeaponValueChanged(WeaponAction action, int value) {
        switch (action) {
            //Reload
            case WeaponAction.Reload: {
                reloadIndicator.SetActive(value >= 0);
                reloadSlider.value = value;
                break;   
            }

            //Abilities
            default: {
                //Get badge
                GameObject badge = action switch {
                    WeaponAction.Primary => primaryValueBadge,
                    WeaponAction.Secondary => secondaryValueBadge,
                    _ => passiveValueBadge,
                };

                //Get text
                TMP_Text text = action switch {
                    WeaponAction.Primary => primaryValueText,
                    WeaponAction.Secondary => secondaryValueText,
                    _ => passiveValueText,
                };

                //Update value
                badge.SetActive(value > 0);
                text.SetText($"{value}");
                break;
            }
        }
    }

    private void OnObtainItem(PassiveItemObject item) {
        itemIcon.sprite = item.Icon;
        itemName.text = item.Name;
        itemDesc.text = item.Description;

        itemAnimator.SetTrigger("Show");
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

        //Add player health change event & update
        Player.AddOnHealthChanged(UpdateHealthIndicator);
        UpdateHealthIndicator(Player.Health);

        //Add weapon change event
        Player.Loadout.AddOnWeaponChanged(OnWeaponChanged);

        //Add item obtain event
        Player.Loadout.AddOnObtainItem(OnObtainItem);

        //Disable minimap when in lobby
        if (Level.IsLobby) minimap.SetActive(false);
    }

    protected override void OnClose() {
        base.OnClose();

        //Not playing
        if (!Application.isPlaying) return;

        //Not in game
        if (!Game.InGame) return;

        //Remove player health change event
        Player.AddOnHealthChanged(UpdateHealthIndicator);

        //Remove weapon change event
        Player.Loadout.RemoveOnWeaponChanged(OnWeaponChanged);

        //Add item obtain event
        Player.Loadout.RemoveOnObtainItem(OnObtainItem);
    }


      /*$$$$$
     /$$__  $$
    | $$  \__/  /$$$$$$  /$$    /$$ /$$$$$$   /$$$$$$
    | $$       /$$__  $$|  $$  /$$//$$__  $$ /$$__  $$
    | $$      | $$  \ $$ \  $$/$$/| $$$$$$$$| $$  \__/
    | $$    $$| $$  | $$  \  $$$/ | $$_____/| $$
    |  $$$$$$/|  $$$$$$/   \  $/  |  $$$$$$$| $$
     \______/  \______/     \_/    \_______/|_*/

    protected override void OnCovered() {
        //Don't do nothin
    }

    protected override void OnUncovered() {
        //Don't do nothin
    }

}
