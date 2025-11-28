using System;
using System.Collections.Generic;
using Botpa;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerUpgrade {
    Weight,     //Extra max health
    Rugosity,   //Resists a % of damage
    Dash        //Reduces dash cooldown
}

public class Player : Character, ISavable {

    //Divider
    [Header("________________"), Space(10)]

    //Player
    [Header("Player")]
    [SerializeField] private Level _level;
    [SerializeField] private Loadout _loadout;
    [SerializeField] private CharacterController controller;

    private Transform cameraTransform;

    public Level Level => _level;
    public CameraController CameraController => Level.CameraController;
    public MenuManager MenuManager => Game.Current.MenuManager;
    public Loadout Loadout => _loadout;

    //Health
    public override float HealthMax => base.HealthMax + (UpgradeGramaje.Level - 1) * gramajeHealthPerLevel;

    //Movement & Rotation
    [Header("Movement & Rotation")]
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private float dashCooldown = 2.5f;
    [SerializeField] private float dashCooldownPerLevel = 0.3f;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float moveSpeed = 6.5f;
    [SerializeField] private float pushDeceleration = 50f;

    private readonly HashSet<object> controlBlockers = new();
    private readonly Timer dashTimer = new();
    private Vector3 pushVelocity;
    private bool isMoving;
    private Vector3 moveDirection;

    public bool IsControlled => controlBlockers.Count == 0;
    public float DashCooldown => 1 - dashTimer.Percent; //0 -> No cooldown, 1 -> Full cooldown

    public override Vector3 MoveVelocity => moveSpeed * SpeedMultiplier * moveDirection;
    public Vector3 FullMoveVelocity => MoveVelocity + pushVelocity;

    //Upgrades
    [Header("Upgrades")]
    [SerializeField] private float gramajeHealthPerLevel = 10;
    [SerializeField] private float rugosidadResistancePerLevel = 0.1f;

    public Upgrade UpgradeGramaje { get; private set; } = new("GRAMAJE");
    public Upgrade UpgradeRugosidad { get; private set; } = new("RUGOSIDAD");
    public Upgrade UpgradeDash { get; private set; } = new("DASH");

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference spinAction;
    [SerializeField] private InputActionReference primaryAction;
    [SerializeField] private InputActionReference secondaryAction;
    [SerializeField] private InputActionReference reloadAction;
    [SerializeField] private InputActionReference dashAction;

    private Vector2 moveInput, lookInput;
    private readonly Timer primaryCoyote = new();
    private readonly Timer secondaryCoyote = new();
    private readonly Timer reloadCoyote = new();

    private const float INPUT_COYOTE_DURATION = 0.25f;
    private bool isLastInputGamepad = false;


    //State
    private void Start() {
        //Get transforms
        cameraTransform = CameraController.Camera.transform;

        //Menu events
        Game.AddOnLoadingChanged(OnGameLoadingChanged);
        OnMenuChanged(MenusList.None, MenuManager.CurrentMenuName);
        MenuManager.AddOnMenuChanged(OnMenuChanged);
        MenuManager.AddOnTransitionStart(OnMenuTransitionStart);
    }

    private void OnDestroy() {
        //Events
        Game.RemoveOnLoadingChanged(OnGameLoadingChanged);
        MenuManager.RemoveOnMenuChanged(OnMenuChanged);
        MenuManager.RemoveOnTransitionStart(OnMenuTransitionStart);
    }

    protected override void OnUpdate() {
        //Player not controlled
        if (!IsControlled) return;

        //Check if switched to gamepad
        if (moveAction.action.activeControl != null) isLastInputGamepad = moveAction.action.activeControl?.device is Gamepad;


         /*$                           /$$      
        | $$                          | $$      
        | $$        /$$$$$$   /$$$$$$ | $$   /$$
        | $$       /$$__  $$ /$$__  $$| $$  /$$/
        | $$      | $$  \ $$| $$  \ $$| $$$$$$/ 
        | $$      | $$  | $$| $$  | $$| $$_  $$ 
        | $$$$$$$$|  $$$$$$/|  $$$$$$/| $$ \  $$
        |________/ \______/  \______/ |__/  \_*/

        //Check input type
        if (isLastInputGamepad) {//Using gamepad
            //Get look input
            lookInput = spinAction.ReadValue<Vector2>();

            //Check if using look input
            if (!lookInput.IsEmpty()) {
                //Look towards look direction
                float angle = Vector2.SignedAngle(Vector2.up, lookInput);
                Model.rotation = Quaternion.Euler(Model.rotation.eulerAngles.x, angle, Model.rotation.eulerAngles.z);
            } else if (isMoving) {
                //Look towards move direction
                float angle = Vector2.SignedAngle(Vector2.up, moveInput * new Vector2(-1, 1));
                Model.rotation = Quaternion.Euler(Model.rotation.eulerAngles.x, angle, Model.rotation.eulerAngles.z);
            }
        } else { //Using Keyboard & mouse
            //Get look input
            lookInput = lookAction.ReadValue<Vector2>();

            //Get player position
            Vector3 playerPosition = transform.position;

            //Try to get mouse world point in player plane
            Plane plane = new(Vector3.up, playerPosition + new Vector3(0, controller.height / 2, 0));
            Ray ray = CameraController.Camera.ScreenPointToRay(lookInput);
            if (plane.Raycast(ray, out float distance)) {
                //Success -> Get point & look towards it
                var hitPoint = ray.GetPoint(distance);
                hitPoint.y = playerPosition.y;
                Model.LookAt(hitPoint, Vector3.up);
            }
        }


         /*$      /$$                              
        | $$$    /$$$                              
        | $$$$  /$$$$  /$$$$$$  /$$    /$$ /$$$$$$ 
        | $$ $$/$$ $$ /$$__  $$|  $$  /$$//$$__  $$
        | $$  $$$| $$| $$  \ $$ \  $$/$$/| $$$$$$$$
        | $$\  $ | $$| $$  | $$  \  $$$/ | $$_____/
        | $$ \/  | $$|  $$$$$$/   \  $/  |  $$$$$$$
        |__/     |__/ \______/     \_/    \______*/

        //Get move input
        moveInput = moveAction.ReadValue<Vector2>();
        isMoving = moveInput.sqrMagnitude > 0;

        //Calculate move direction
        moveDirection = isMoving ? Vector3.ProjectOnPlane(moveInput.x * cameraTransform.right + moveInput.y * cameraTransform.forward, Vector3.up).normalized : Vector3.zero;

        //Check for dash
        if (dashAction.Triggered() && !dashTimer.IsCounting) {
            //Start dash timer
            dashTimer.Count(dashCooldown - (UpgradeDash.Level - 1) * dashCooldownPerLevel);

            //Play dash sound
            PlaySound(dashSound);

            //Push player (dash)
            Vector3 direction = isMoving ? moveDirection : Model.forward;
            Push(dashForce * direction);

            //Dash item hooks
            foreach (var pair in Loadout.PassiveItems) pair.Key.OnDashHook(this, pair.Value, direction);
        }
    
        //Move in move direction
        controller.SimpleMove(FullMoveVelocity);

        //Decrease push velocity
        pushVelocity = Vector3.MoveTowards(pushVelocity, Vector3.zero, Time.deltaTime * pushDeceleration);


          /*$$$$$              /$$     /$$
         /$$__  $$            | $$    |__/
        | $$  \ $$  /$$$$$$$ /$$$$$$   /$$  /$$$$$$  /$$$$$$$   /$$$$$$$
        | $$$$$$$$ /$$_____/|_  $$_/  | $$ /$$__  $$| $$__  $$ /$$_____/
        | $$__  $$| $$        | $$    | $$| $$  \ $$| $$  \ $$|  $$$$$$
        | $$  | $$| $$        | $$ /$$| $$| $$  | $$| $$  | $$ \____  $$
        | $$  | $$|  $$$$$$$  |  $$$$/| $$|  $$$$$$/| $$  | $$ /$$$$$$$/
        |__/  |__/ \_______/   \___/  |__/ \______/ |__/  |__/|______*/

        //Check for action inputs
        if (primaryAction.Triggered()) primaryCoyote.Count(INPUT_COYOTE_DURATION);
        if (secondaryAction.Triggered()) secondaryCoyote.Count(INPUT_COYOTE_DURATION);
        if (reloadAction.Triggered()) reloadCoyote.Count(INPUT_COYOTE_DURATION);

        //Check if an action should be performed
        if (primaryCoyote.IsCounting && Loadout.UsePrimary()) primaryCoyote.Reset();
        if (secondaryCoyote.IsCounting && Loadout.UseSecondary()) secondaryCoyote.Reset();
        if (reloadCoyote.IsCounting && Loadout.Reload()) reloadCoyote.Reset();


          /*$$$$$            /$$                           /$$              
         /$$__  $$          |__/                          | $$              
        | $$  \ $$ /$$$$$$$  /$$ /$$$$$$/$$$$   /$$$$$$  /$$$$$$    /$$$$$$ 
        | $$$$$$$$| $$__  $$| $$| $$_  $$_  $$ |____  $$|_  $$_/   /$$__  $$
        | $$__  $$| $$  \ $$| $$| $$ \ $$ \ $$  /$$$$$$$  | $$    | $$$$$$$$
        | $$  | $$| $$  | $$| $$| $$ | $$ | $$ /$$__  $$  | $$ /$$| $$_____/
        | $$  | $$| $$  | $$| $$| $$ | $$ | $$|  $$$$$$$  |  $$$$/|  $$$$$$$
        |__/  |__/|__/  |__/|__/|__/ |__/ |__/ \_______/   \___/   \______*/

        //Animate
        Animator.SetBool("IsMoving", isMoving);
    }

    //Health
    protected override void OnDeath() {
        base.OnDeath();

        //Death item hooks
        foreach (var pair in Loadout.PassiveItems) pair.Key.OnDeathHook(this, pair.Value);
        Loadout.RemoveQueuedPassiveItems();

        //Run the alive check again since items could have altered that outcome
        if (!IsAlive) MenuManager.Open(MenusList.Death);
    }

    public override bool Damage(float amount, DamageType type, object source) {
        //Calculate resistance
        float resistance = amount * (UpgradeRugosidad.Level - 1) * rugosidadResistancePerLevel;

        //Hurt item hooks
        foreach (var pair in Loadout.PassiveItems) pair.Key.OnHurtHook(this, pair.Value, (source is Character character) ? character : null);

        //Damage
        bool success = base.Damage(amount - resistance, type, source);

        //Return success
        return success;
    }

    //Movement
    private void StopMovement() {
        isMoving = false;
        Animator.SetBool("IsMoving", isMoving);
    }

    public override void TeleportTo(Vector3 position) {
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }

    public override void Push(Vector3 direction) {
        pushVelocity += direction;
    }

    public void BlockControls(object obj) {
        controlBlockers.Add(obj);
        StopMovement();
    }

    public void UnblockControls(object obj) {
        controlBlockers.Remove(obj);
    }

    //Upgrades
    public Upgrade GetUpgrade(PlayerUpgrade type) {
        return type switch  {
            PlayerUpgrade.Weight => UpgradeGramaje,
            PlayerUpgrade.Rugosity => UpgradeRugosidad,
            _ => UpgradeDash
        };
    }

    public bool TryUpgrade(PlayerUpgrade type) {
        //Try to upgrade
        bool upgraded = GetUpgrade(type).TryUpgrade(Loadout);
        if (!upgraded) return false;

        //Check for upgrade changes
        switch (type) {
            //Reset player health
            case PlayerUpgrade.Weight:
                Heal(HealthMax);
                break;
        }

        //Success
        return true;
    }

    //Events (other)
    private void OnGameLoadingChanged(bool IsLoading) {
        if (IsLoading)
            BlockControls("LOADING");
        else
            UnblockControls("LOADING");
    }

    private void OnMenuChanged(string oldMenu, string newMenu) {
        //Change POV depending on menu
        switch (newMenu) {
            //Escapist
            case MenusList.Game:
                //Control player
                UnblockControls("MENU_TRANSITION");
                break;

            //Other
            default:
                //Don't control player
                BlockControls("MENU_TRANSITION");

                //Stop animations
                StopMovement();
                break;
        }
    }

    private void OnMenuTransitionStart(string oldMenu, string newMenu) {
        //Menu transition started -> Stop player
        StopMovement();
    }

    //Saving
    public string OnSave() {
        return JsonUtility.ToJson(new PlayerSave() {
            //Loadout
            loadout = Loadout.OnSave(),
            //Health
            health = Health
        });
    }

    public void OnLoad(string saveJson) {
        //Parse save
        var save = JsonUtility.FromJson<PlayerSave>(saveJson);

        //Load loadout
        Loadout.OnLoad(save.loadout);

        //Load upgrades
        UpgradeGramaje.SetLevel(Loadout.GetUpgrade(UpgradeGramaje.Key));
        UpgradeRugosidad.SetLevel(Loadout.GetUpgrade(UpgradeRugosidad.Key));
        UpgradeDash.SetLevel(Loadout.GetUpgrade(UpgradeDash.Key));

        //Load health
        Health = Level.IsLobby ? HealthMax : save.health;
        CallOnHealthChanged();
    }

    [Serializable]
    private class PlayerSave {

        //Loadout
        public string loadout = "{}";

        //Health
        public float health = DEFAULT_HEALTH_MAX;

    }

}
