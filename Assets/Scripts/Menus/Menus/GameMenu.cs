using System.Collections;
using System.Collections.Generic;
using Botpa;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private Animator damageIndicator;
    [SerializeField] private TMP_Text healthText;

    private float lastKnownHealth = -1;

    //Weapon
    private Weapon currentWeapon;

    //Actions
    [Header("Actions")]
    [SerializeField] private GameMenuAction primaryAction;
    [SerializeField] private GameMenuAction secondaryAction;
    [SerializeField] private GameMenuAction passiveAction;
    [SerializeField] private GameMenuAction dashAction;

    //Reload
    [Header("Reload")]
    [SerializeField] private GameObject reloadIndicator;
    [SerializeField] private Slider reloadSlider;

    //Messages
    [Header("Messages")]
    [SerializeField] private Animator messageAnimator;
    [SerializeField] private TMP_Text messageText;

    private readonly List<string> messages = new();

    public bool ShowingMessage { get; private set; } = false;

    //Item
    [Header("Passive Item")]
    [SerializeField] private Animator itemAnimator;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDesc;

    //Treasure
    [Header("Treasure Item")]
    [SerializeField] private Animator treasureAnimator;
    [SerializeField] private Image treasureIcon;
    [SerializeField] private TMP_Text treasureName;

    //Minimap
    [Header("Minimap")]
    [SerializeField] private GameObject minimap;
    [SerializeField] private Transform minimapCamera;

    //Boss Healthbar
    [Header("Boss Healthbar")]
    [SerializeField] private GameObject bossHealthbar;
    [SerializeField] private Image bossHealthbarFill;

    private Enemy boss;


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
            primaryAction.SetCooldown(currentWeapon.PrimaryCooldown);
            secondaryAction.SetCooldown(currentWeapon.SecondaryCooldown);
            passiveAction.SetCooldown(currentWeapon.PassiveCooldown);
        }

        //Update dash cooldown
        dashAction.SetCooldown(Player.DashCooldown);
    }

    public override void OnUpdate() {
        //Open pause
        if (pauseAction.Triggered()) OpenPause();
        //Open inventory
        else if (inventoryAction.Triggered()) OpenInventory();

        //Disable minimap in lobby
        minimap.SetActive(!Level.IsLobby);

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
    private void OnHealthChanged(float health, float healthMax) {
        //Check if health is init
        if (lastKnownHealth >= 0) damageIndicator.SetTrigger("Damage");

        //Update health UI
        UpdateHealthIndicator(health);

        //Update health
        lastKnownHealth = health;
    }

    private void UpdateHealthIndicator(float health) {
        //Update health UI
        healthText.SetText($"{health}HP");
    }

    //Weapon
    private void OnWeaponChanged(Weapon oldWeapon, Weapon newWeapon) {
        //No weapon -> Ignore
        if (!newWeapon) return;

        //Update current weapon
        currentWeapon = newWeapon;

        //Update icons
        primaryAction.SetSprite(newWeapon.PrimaryIcon);
        secondaryAction.SetSprite(newWeapon.SecondaryIcon);
        passiveAction.SetSprite(newWeapon.PassiveIcon);

        //Update cooldowns
        primaryAction.SetCooldown(newWeapon.PrimaryCooldown);
        secondaryAction.SetCooldown(newWeapon.SecondaryCooldown);
        passiveAction.SetCooldown(newWeapon.PassiveCooldown);

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
        if (action == WeaponAction.Reload) {
            //Reload
            reloadIndicator.SetActive(value >= 0);
            reloadSlider.value = value;
        } else {
            //Abilities
            (action switch {
                WeaponAction.Primary => primaryAction,
                WeaponAction.Secondary => secondaryAction,
                _ => passiveAction,
            }).SetValue(value > 0, $"{value}");
        }
    }

    private void OnObtainItem(PassiveItemObject item) {
        itemIcon.sprite = item.Icon;
        itemName.text = item.Name;
        itemDesc.text = item.Description;

        itemAnimator.SetTrigger("Show");
    }

    //Boss healthbar
    private void OnBossHealthChange(float health, float healthMax) {
        bossHealthbarFill.fillAmount = health / healthMax;
        bossHealthbar.SetActive(health > 0);
    }

    public void AssignBoss(Enemy enemy) {
        boss = enemy;
        boss.AddOnHealthChanged(OnBossHealthChange);
        OnBossHealthChange(boss.Health, boss.HealthMax);
    }

    private void OnObtainTreasure(Item item) {
        treasureIcon.sprite = item.Icon;
        treasureName.text = $"{item.Name} x1";

        treasureAnimator.SetTrigger("Show");
    }

    //Messages
    private IEnumerator ShowMessageCoroutine() {
        //Start
        ShowingMessage = true;

        //Show message
        messageText.SetText(messages[0]);
        messageAnimator.SetTrigger("Show");
        messages.RemoveAt(0);

        //Wait until animation finishes
        yield return new WaitForSeconds(3);

        //Check if there are more messages
        if (!messages.IsEmpty()) {
            //More -> Show next
            StartCoroutine(ShowMessageCoroutine());
        } else {
            //Finish
            ShowingMessage = false;
        }
    }

    public void ShowMessage(string message) {
        //Add message to list
        messages.Add(message);

        //Show message if not showing any yet
        if (!ShowingMessage) StartCoroutine(ShowMessageCoroutine());
    }

    //Menus
    public void OpenPause() {
        MenuManager.Open(MenusList.Settings);
    }

    public void OpenInventory() {
        MenuManager.Open(MenusList.Inventory);
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

        //Add player health change event & update it
        Player.AddOnHealthChanged(OnHealthChanged);
        UpdateHealthIndicator(Player.Health);
        lastKnownHealth = -1;

        //Add weapon change event
        Player.Loadout.AddOnWeaponChanged(OnWeaponChanged);

        //Add item obtain event
        Player.Loadout.AddOnObtainItem(OnObtainItem);

        //Hide boss healthbar
        bossHealthbar.SetActive(false);
        
        //Add treasure obtain event
        Player.Loadout.AddOnObtainTreasure(OnObtainTreasure);

        //Show tutorial
        /*if (!Preferences.TutorialShown) {
            Preferences.TutorialShown = true;
            MenuManager.Open(MenusList.Tutorial);
        }*/
    }

    protected override void OnClose() {
        base.OnClose();

        //Not playing
        if (!Application.isPlaying) return;

        //Not in game
        if (!Game.InGame) return;

        //Remove player health change event
        Player.RemoveOnHealthChanged(OnHealthChanged);

        //Remove weapon change event
        Player.Loadout.RemoveOnWeaponChanged(OnWeaponChanged);

        //Remove item obtain event
        Player.Loadout.RemoveOnObtainItem(OnObtainItem);

        //Remove boss health change event
        if (boss) boss.RemoveOnHealthChanged(OnBossHealthChange);
        
        //Remove treasure obtain event
        Player.Loadout.RemoveOnObtainTreasure(OnObtainTreasure);
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
