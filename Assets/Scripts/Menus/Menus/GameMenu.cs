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

    //Effects
    [Header("Effects")]
    [SerializeField] private Image damageIndicator;

    //Ammo
    [Header("Ammo")]
    [SerializeField] private GameObject ammoIndicator;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Item ammoItem;

    //Player
    private Transform Top => Player.Top;


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

    //Effects
    private void UpdateDamageIndicator(float health) {
        Color color = damageIndicator.color;
        color.a = Ease.OutCubic(1f - health / Character.MAX_HEALTH);
        damageIndicator.color = color;
    }

    //Ammo
    private void UpdateAmmo() {
        /*
        //Gather info
        int ammoCount = 0;//Arms.AmmoCount;
        bool show = ammoCount >= 0;

        //Update ammo
        ammoIndicator.SetActive(show);
        if (show) ammoText.SetText("" + ammoCount);
        */
    }

    private void OnItemChanged(Item item, int amount) {
        if (item == ammoItem) UpdateAmmo();
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
        Level.Player.AddOnHealthChanged(UpdateDamageIndicator);
        UpdateDamageIndicator(Level.Player.Health);
        
        //Update ammo
        UpdateAmmo();

        //Add item change event
        //Inventory.AddOnItemChanged(OnItemChanged);
    }

    protected override void OnClose() {
        base.OnClose();
        
        //Remove player health change event
        Level.Player.AddOnHealthChanged(UpdateDamageIndicator);

        //Remove item change event
        //Inventory.RemoveOnItemChanged(OnItemChanged);
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
        //Show cursor
        Game.UnhideCursor(this);
    }

    protected override void OnUncovered() {
        //Hide cursor
        Game.HideCursor(this);
    }

}
