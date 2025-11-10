using System;
using System.Collections.Generic;
using Botpa;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerUpgrade {
    Gramaje,    //Extra max health
    Rugosidad   //Resists a % of damage
}

public class Player : Character, ISavable {

    //Level
    [Header("Level")]
    [SerializeField] private Level _level;

    public Level Level => _level;
    public CameraController CameraController => Level.CameraController;
    public MenuManager MenuManager => Game.Current.MenuManager;

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

    //Components
    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Loadout _loadout;
    [SerializeField] private GameObject model;
    [SerializeField] private Animator _animator;

    private Transform cameraTransform;

    public Loadout Loadout => _loadout;
    public Animator Animator => _animator;

    //Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6.5f;
    [SerializeField] private float dashCooldown = 2.5f;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float pushDeceleration = 50f;

    private readonly Timer dashTimer = new();
    private Transform playerTransform;
    private Vector3 pushVelocity;
    private bool isMoving;
    private Vector3 moveDirection;

    //Controls
    private readonly HashSet<object> controlBlockers = new();

    public bool IsControlled => controlBlockers.Count == 0;

    //Upgrades
    [Header("Upgrades")]
    [SerializeField] private float gramajeHealthPerLevel = 10;
    [SerializeField] private float rugosidadResistancePerLevel = 0.1f;

    public Upgrade GramajeUpgrade { get; private set; } = new("GRAMAJE");
    public Upgrade RugosidadUpgrade { get; private set; } = new("RUGOSIDAD");

    //Health
    public override float HealthMax => base.HealthMax + (GramajeUpgrade.Level - 1) * gramajeHealthPerLevel;


    //State
    private void Start() {
        //Get transforms
        cameraTransform = CameraController.Camera.transform;
        playerTransform = transform;

        //Events
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
        if (moveAction.action.activeControl != null) {
            isLastInputGamepad = moveAction.action.activeControl?.device is Gamepad;
        }


         /*$                           /$$      
        | $$                          | $$      
        | $$        /$$$$$$   /$$$$$$ | $$   /$$
        | $$       /$$__  $$ /$$__  $$| $$  /$$/
        | $$      | $$  \ $$| $$  \ $$| $$$$$$/ 
        | $$      | $$  | $$| $$  | $$| $$_  $$ 
        | $$$$$$$$|  $$$$$$/|  $$$$$$/| $$ \  $$
        |________/ \______/  \______/ |__/  \_*/


        if (isLastInputGamepad) // Using gamepad
        {
            //Get look input
            lookInput = spinAction.ReadValue<Vector2>();

            if (lookInput != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(Vector2.up, lookInput);
                playerTransform.rotation = Quaternion.Euler(playerTransform.rotation.eulerAngles.x, angle, playerTransform.rotation.eulerAngles.z);
            }
            else if (isMoving)
            {
                float angle = Vector2.SignedAngle(Vector2.up, moveInput * new Vector2(-1, 1));
                playerTransform.rotation = Quaternion.Euler(playerTransform.rotation.eulerAngles.x, angle, playerTransform.rotation.eulerAngles.z);
            }
        }
        else // Using Keyboard & mouse
        {
            //Get look input
            lookInput = lookAction.ReadValue<Vector2>();

            //Try to get mouse world point in player plane
            var plane = new Plane(Vector3.up, playerTransform.position + new Vector3(0, controller.height / 2, 0));
            var ray = CameraController.Camera.ScreenPointToRay(lookInput);
            if (plane.Raycast(ray, out float distance))
            {
                //Success -> Get point & look towards it
                var hitPoint = ray.GetPoint(distance);
                hitPoint.y = playerTransform.position.y;
                playerTransform.LookAt(hitPoint, Vector3.up);
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
        if (dashAction.Triggered() && !dashTimer.counting) {

            //Start dash timer
            dashTimer.Count(dashCooldown);

            //Push player (dash)
            Vector3 direction = isMoving ? moveDirection : playerTransform.forward;
            Push(dashForce * direction);

            //Dash item hooks
            foreach (var pair in Loadout.PassiveItems) pair.Key.OnDashHook(this, pair.Value, direction);
        }
    
        //Move in move direction
        controller.SimpleMove(moveSpeed * SpeedMultiplier * moveDirection + pushVelocity);

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
        if (primaryCoyote.counting && Loadout.UsePrimary()) primaryCoyote.Reset();
        if (secondaryCoyote.counting && Loadout.UseSecondary()) secondaryCoyote.Reset();
        if (reloadCoyote.counting && Loadout.Reload()) reloadCoyote.Reset();


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

    //Movement
    public override void TeleportTo(Vector3 position) {
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }

    public override void Push(Vector3 direction) {
        pushVelocity += direction;
    }

    private void StopMovement() {
        isMoving = false;
        Animator.SetBool("IsMoving", isMoving);
    }

    //Controls
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
            PlayerUpgrade.Gramaje => GramajeUpgrade,
            _ => RugosidadUpgrade
        };
    }

    public bool TryUpgrade(PlayerUpgrade type) {
        //Try to upgrade
        bool upgraded = GetUpgrade(type).TryUpgrade(Loadout);
        if (!upgraded) return false;

        //Check for upgrade changes
        switch (type) {
            //Reset player health
            case PlayerUpgrade.Gramaje:
                Heal(HealthMax);
                break;
        }

        //Success
        return true;
    }

    //Health
    public override bool Damage(float amount, object source, DamageType type = DamageType.None) {
        //Calculate resistance
        float resistance = amount * (RugosidadUpgrade.Level - 1) * rugosidadResistancePerLevel;

        //Hurt item hooks
        foreach (var pair in Loadout.PassiveItems) pair.Key.OnHurtHook(this, pair.Value, (source is Character character) ? character : null);

        //Damage
        bool success = base.Damage(amount - resistance, source, type);

        //Return success
        return success;
    }

    protected override void OnDeath() {
        //Death item hooks
        foreach (var pair in Loadout.PassiveItems) pair.Key.OnDeathHook(this, pair.Value);

        //Run the alive check again since items could have altered that outcome
        if (!IsAlive) MenuManager.Open(MenusList.Death);
    }

    public float GetBaseHealth()
    {
        return DEFAULT_HEALTH_MAX;
    }

    public Vector3 GetVelocity()
    {
        return moveSpeed * SpeedMultiplier * moveDirection;
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
        GramajeUpgrade.SetLevel(Loadout.GetUpgrade(GramajeUpgrade.Key));
        RugosidadUpgrade.SetLevel(Loadout.GetUpgrade(RugosidadUpgrade.Key));

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
