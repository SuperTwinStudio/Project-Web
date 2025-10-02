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

    //Player class
    [Header("Player class")]
    [SerializeField] private Image classPrimaryIcon;
    [SerializeField] private Image classSecondaryIcon;


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
    private void OnClassChanged(PlayerClass c) {
        //Update icons n stuff
        classPrimaryIcon.sprite = c.Item.Icon;
        classSecondaryIcon.sprite = c.Item.Icon;
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
