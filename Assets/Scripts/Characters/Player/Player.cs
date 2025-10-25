using System;
using System.Collections.Generic;
using System.Linq;
using Botpa;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerUpgrade
{
    Gramaje,    //Extra max health
    Rugosidad   //Resists a % of damage
}

public class Player : Character, ISavable
{

    //Character
    public override float HealthMax => HEALTH_MAX + (GramajeUpgrade.Level - 1) * gramajeHealthPerLevel;

    //Level
    [Header("Level")]
    [SerializeField] private Level _level;

    public Level Level => _level;
    public MenuManager MenuManager => Level.MenuManager;
    public CameraController CameraController => Level.CameraController;

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference spinAction;
    [SerializeField] private InputActionReference primaryAction;
    [SerializeField] private InputActionReference secondaryAction;
    [SerializeField] private InputActionReference testAction;

    private Vector2 moveInput, lookInput;
    private readonly Timer primaryCoyote = new();
    private readonly Timer secondaryCoyote = new();

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
    [SerializeField] private float moveSpeed = 5f;

    private Transform playerTransform;

    private bool isControlled, isMoving;

    //Upgrades
    [Header("Upgrades")]
    [SerializeField] private float gramajeHealthPerLevel = 10;
    [SerializeField] private float rugosidadResistancePerLevel = 0.1f;

    public Upgrade GramajeUpgrade { get; private set; } = new("gramaje", 1, Upgrade.DEFAULT_MAX_LEVEL);
    public Upgrade RugosidadUpgrade { get; private set; } = new("rugosidad", 1, Upgrade.DEFAULT_MAX_LEVEL);

    //Effects
    private readonly Dictionary<Effect, float> effects = new();
    private float slowSpeedMultiplier = 1;


    //State
    private void Start()
    {
        //Get transforms
        cameraTransform = CameraController.Camera.transform;
        playerTransform = transform;

        //Refresh upgrade levels
        GramajeUpgrade.SetLevel(Loadout.GetUpgrade(GramajeUpgrade.Key));
        RugosidadUpgrade.SetLevel(Loadout.GetUpgrade(RugosidadUpgrade.Key));

        //Events
        OnMenuChanged(MenusList.None, MenuManager.CurrentMenuName);
        MenuManager.AddOnMenuChanged(OnMenuChanged);
        MenuManager.AddOnTransitionStart(OnMenuTransitionStart);
    }

    private void Update()
    {
        //Game is paused | player is dead | player is not controlled | a menu is transitioning
        if (Game.IsPaused || !IsAlive || !isControlled || MenuManager.InTransition) return;

        //Update effects
        UpdateEffects();

        //Test (damage player)
        if (testAction.Triggered()) Damage(10);

        //Check if switched to gamepad
        if (moveAction.action.activeControl != null)
        {
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

        //Moving
        if (isMoving)
        {
            //Calculate move direction
            Vector3 moveDirection = Vector3.ProjectOnPlane(moveInput.x * cameraTransform.right + moveInput.y * cameraTransform.forward, Vector3.up).normalized;

            //Move in move direction
            controller.SimpleMove(moveSpeed * moveDirection);
        }


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

        //Check if an action should be performed
        if (primaryCoyote.counting && Loadout.UsePrimary()) primaryCoyote.Reset();
        if (secondaryCoyote.counting && Loadout.UseSecondary()) secondaryCoyote.Reset();


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

    private void StopMovement()
    {
        isMoving = false;
        Animator.SetBool("IsMoving", isMoving);
    }

    //Effects
    private void UpdateEffects()
    {
        //Get current time
        float nowTimestamp = Time.time;

        //Reset slow multiplier
        slowSpeedMultiplier = 1;

        //Apply effects
        foreach (var effect in effects.Keys.ToList())
        {
            //Get end timestamp
            float endTimestamp = effects[effect];

            //Apply effect
            switch (effect.Action.Type)
            {
                //Damage
                case EffectType.Damage:
                    Damage(Time.deltaTime * effect.Action.Points);  //Take points as damage per second
                    break;
                //Heal
                case EffectType.Heal:
                    Heal(Time.deltaTime * effect.Action.Points);    //Take points as healing per second
                    break;
                //Slow
                case EffectType.Slow:
                    slowSpeedMultiplier = Mathf.Min(slowSpeedMultiplier, Mathf.Clamp01(1 - effect.Action.Points));
                    break;
            }

            //Check if effect finished
            if (nowTimestamp > endTimestamp) effects.Remove(effect);
        }
    }

    public void AddEffect(Effect effect, float duration)
    {
        //Calculate effect end timestamp
        float effectEndTimestamp = Time.time + duration;

        //Check if player already has effect
        if (effects.ContainsKey(effect))
        {
            //Already has effect -> Check to update duration
            effects[effect] = Mathf.Max(effects[effect], effectEndTimestamp);
        }
        else
        {
            //Does not have effect -> Add it
            effects[effect] = effectEndTimestamp;
        }
    }

    //Upgrades
    public Upgrade GetUpgrade(PlayerUpgrade type)
    {
        return type switch
        {
            PlayerUpgrade.Gramaje => GramajeUpgrade,
            _ => RugosidadUpgrade
        };
    }

    public bool TryUpgrade(PlayerUpgrade type)
    {
        //Try to upgrade
        bool upgraded = GetUpgrade(type).TryUpgrade(Loadout);
        if (!upgraded) return false;

        //Check for upgrade changes
        switch (type)
        {
            //Reset player health
            case PlayerUpgrade.Gramaje:
                Heal(HealthMax);
                break;
        }

        //Success
        return true;
    }

    //Health
    protected override void OnDeath(bool instant = false)
    {
        MenuManager.Open(MenusList.Death);
    }

    public override bool Damage(float amount)
    {
        //Calculate resistance
        float resistance = amount * (RugosidadUpgrade.Level - 1) * rugosidadResistancePerLevel;

        //Damage
        bool success = base.Damage(amount - resistance);

        //Died -> Open death menu
        if (success && !IsAlive) MenuManager.Open(MenusList.Death);

        //Return success
        return success;
    }

    //Events (other)
    private void OnMenuChanged(string oldMenu, string newMenu)
    {
        //Change POV depending on menu
        switch (newMenu)
        {
            //Escapist
            case MenusList.Game:
                //Control player
                isControlled = true;
                break;

            //Other
            default:
                //Don't control player
                isControlled = false;

                //Stop animations
                StopMovement();
                break;
        }
    }

    private void OnMenuTransitionStart(string oldMenu, string newMenu)
    {
        //Menu transition started -> Stop player
        StopMovement();
    }

    //Saving
    public string OnSave()
    {
        return JsonUtility.ToJson(new PlayerSave()
        {
            //Health
            health = Health,
            //Upgrades
            levelGramaje = GramajeUpgrade.Level,
            levelRugosidad = RugosidadUpgrade.Level,
            //Loadout
            loadout = Loadout.OnSave()
        });
    }

    public void OnLoad(string saveJson)
    {
        //Parse save
        var save = JsonUtility.FromJson<PlayerSave>(saveJson);

        //Load health
        if (Level.IsLobby)
            Health = HealthMax;
        else
            Health = save.health;

        //Load upgrades
        GramajeUpgrade.SetLevel(save.levelGramaje);
        RugosidadUpgrade.SetLevel(save.levelRugosidad);

        //Load loadout
        Loadout.OnLoad(save.loadout);
    }

    [Serializable]
    private class PlayerSave
    {

        //Health
        public float health = HEALTH_MAX;

        //Upgrades
        public int levelGramaje = 1;
        public int levelRugosidad = 1;

        //Loadout
        public string loadout = "{}";

    }

}
