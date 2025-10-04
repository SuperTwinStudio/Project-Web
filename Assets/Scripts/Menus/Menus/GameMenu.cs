using Botpa;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameMenu : Menu {
    
    //Prefab
    public override string Name => "Game";

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference inventoryAction;

    //Health
    [Header("Health")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image damageIndicator;

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


      /*$$$$$   /$$                 /$$
     /$$__  $$ | $$                | $$
    | $$  \__//$$$$$$    /$$$$$$  /$$$$$$    /$$$$$$
    |  $$$$$$|_  $$_/   |____  $$|_  $$_/   /$$__  $$
     \____  $$ | $$      /$$$$$$$  | $$    | $$$$$$$$
     /$$  \ $$ | $$ /$$ /$$__  $$  | $$ /$$| $$_____/
    |  $$$$$$/ |  $$$$/|  $$$$$$$  |  $$$$/|  $$$$$$$
     \______/   \___/   \_______/   \___/   \______*/

    private void Update() {
        //Update shaders that use unscaled time
        damageIndicator.material.SetFloat("_UnscaledTime", Time.unscaledTime);

        //Update icons cooldown
        if (currentWeapon) {
            primaryCooldown.fillAmount = currentWeapon.PrimaryCooldown;
            secondaryCooldown.fillAmount = currentWeapon.SecondaryCooldown;
            passiveCooldown.fillAmount = currentWeapon.PassiveCooldown;
        }
    }

    private void OnDestroy() {
        //Reset shaders that use unscaled time
        damageIndicator.material.SetFloat("_UnscaledTime", 0);
    }

    public override void OnUpdate() {
        //Open inventory
        if (inventoryAction.Triggered()) MenuManager.Open(MenusList.Inventory);
    }
    
    public override bool OnBack() {
        MenuManager.Open(MenusList.Pause); //Pause game
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
        healthText.SetText($"Health: {health}");

        //Damage indicator effect
        Color color = damageIndicator.color;
        color.a = Ease.OutCubic(1f - health / Character.MAX_HEALTH);
        damageIndicator.color = color;
    }

    //Weapon
    private void OnWeaponChanged(Weapon oldWeapon, Weapon newWeapon) {
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

        //Update values
        OnWeaponValueChanged(WeaponType.Primary, newWeapon.PrimaryValue);
        OnWeaponValueChanged(WeaponType.Secondary, newWeapon.SecondaryValue);
        OnWeaponValueChanged(WeaponType.Passive, newWeapon.PassiveValue);
        if (oldWeapon) oldWeapon.RemoveOnValueChanged(OnWeaponValueChanged);
        if (newWeapon) newWeapon.AddOnValueChanged(OnWeaponValueChanged);
    }

    private void OnWeaponValueChanged(WeaponType type, int value) {
        //Get badge
        GameObject badge = type switch {
            WeaponType.Primary => primaryValueBadge,
            WeaponType.Secondary => secondaryValueBadge,
            _ => passiveValueBadge,
        };

        //Get text
        TMP_Text text = type switch {
            WeaponType.Primary => primaryValueText,
            WeaponType.Secondary => secondaryValueText,
            _ => passiveValueText,
        };

        //Update value
        badge.SetActive(value > 0);
        text.SetText($"{value}");
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
        
        //Add player health change event & update
        Player.AddOnHealthChanged(UpdateHealthIndicator);
        UpdateHealthIndicator(Player.Health);
        
        //Add weapon change event
        Player.Loadout.AddOnWeaponChanged(OnWeaponChanged);
    }

    protected override void OnClose() {
        base.OnClose();
        
        //Remove player health change event
        Player.AddOnHealthChanged(UpdateHealthIndicator);

        //Remove weapon change event
        Player.Loadout.RemoveOnWeaponChanged(OnWeaponChanged);
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
