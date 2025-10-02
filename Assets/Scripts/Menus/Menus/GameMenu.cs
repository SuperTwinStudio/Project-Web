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

    //Class
    private PlayerClass currentClass;

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

        //Update icons
        if (currentClass) {
            primaryCooldown.fillAmount = currentClass.PrimaryCooldown;
            secondaryCooldown.fillAmount = currentClass.SecondaryCooldown;
            passiveCooldown.fillAmount = currentClass.PassiveCooldown;
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

    //Player class
    private void OnClassChanged(PlayerClass oldClass, PlayerClass newClass) {
        //Update current class
        currentClass = newClass;

        //Update icons
        primaryIcon.sprite = newClass.Item.Icon;
        secondaryIcon.sprite = newClass.Item.Icon;
        passiveIcon.sprite = newClass.Item.Icon;

        //Update cooldowns
        primaryCooldown.fillAmount = newClass.PrimaryCooldown;
        secondaryCooldown.fillAmount = newClass.SecondaryCooldown;
        passiveCooldown.fillAmount = newClass.PassiveCooldown;

        //Update values
        OnClassValueChanged(ClassType.Primary, newClass.PrimaryValue);
        OnClassValueChanged(ClassType.Secondary, newClass.SecondaryValue);
        OnClassValueChanged(ClassType.Passive, newClass.PassiveValue);
        if (oldClass) oldClass.RemoveOnValueChanged(OnClassValueChanged);
        if (newClass) newClass.AddOnValueChanged(OnClassValueChanged);
    }

    private void OnClassValueChanged(ClassType type, int value) {
        //Get badge
        GameObject badge = type switch {
            ClassType.Primary => primaryValueBadge,
            ClassType.Secondary => secondaryValueBadge,
            _ => passiveValueBadge,
        };

        //Get text
        TMP_Text text = type switch {
            ClassType.Primary => primaryValueText,
            ClassType.Secondary => secondaryValueText,
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
        
        //Add class change event
        Player.Loadout.AddOnClassChanged(OnClassChanged);
    }

    protected override void OnClose() {
        base.OnClose();
        
        //Remove player health change event
        Player.AddOnHealthChanged(UpdateHealthIndicator);

        //Remove class change event
        Player.Loadout.RemoveOnClassChanged(OnClassChanged);
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
