using Botpa;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character {

    //Level
    [Header("Level")]
    [SerializeField] private Level _level;

    public Level Level => _level;
    public MenuManager MenuManager => Level.MenuManager;

    //Input
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference primaryAction;
    [SerializeField] private InputActionReference secondaryAction;

    private Vector2 moveInput, lookInput;

    //Components
    [Header("Components")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject model;
    [SerializeField] private Animator animator;

    //Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 0.8f;

    private bool isControlled, isMoving, isRunning;

    private const float MAX_ROTATION_DELTA = 700;


    //State
    private void Start() {
        //Events
        OnMenuChanged(MenusList.None, MenuManager.CurrentMenuName);
        MenuManager.AddOnMenuChanged(OnMenuChanged);
        MenuManager.AddOnTransitionStart(OnMenuTransitionStart);
    }

    private void Update() {
        //Game is paused | player is not controlled | menu is in transition
        if (Game.IsPaused || !isControlled || MenuManager.InTransition) return;


         /*$                           /$$      
        | $$                          | $$      
        | $$        /$$$$$$   /$$$$$$ | $$   /$$
        | $$       /$$__  $$ /$$__  $$| $$  /$$/
        | $$      | $$  \ $$| $$  \ $$| $$$$$$/ 
        | $$      | $$  | $$| $$  | $$| $$_  $$ 
        | $$$$$$$$|  $$$$$$/|  $$$$$$/| $$ \  $$
        |________/ \______/  \______/ |__/  \_*/
        
        //Get look input
        lookInput = lookAction.ReadValue<Vector2>();

        //Rotate horizontally
        /*transform.Rotate(Vector3.up, lookInput.x);

        //Rotate vertically
        headVerticalRotation = Mathf.Clamp(headVerticalRotation - lookInput.y * Preferences.MouseSensitivity, -80, 80);
        Vector3 clampedVerticalRotation = Top.localEulerAngles;
        clampedVerticalRotation.x = headVerticalRotation;
        Top.localEulerAngles = clampedVerticalRotation;*/


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
        if (isMoving) {
            //Calculate move direction
            Vector3 moveDirection = Vector3.ProjectOnPlane(moveInput.x * cameraTransform.right + moveInput.y * cameraTransform.forward, Vector3.up).normalized;
            isRunning = false;

            //Move in move direction
            controller.SimpleMove(moveSpeed * moveDirection);
        }


          /*$$$$$            /$$                           /$$              
         /$$__  $$          |__/                          | $$              
        | $$  \ $$ /$$$$$$$  /$$ /$$$$$$/$$$$   /$$$$$$  /$$$$$$    /$$$$$$ 
        | $$$$$$$$| $$__  $$| $$| $$_  $$_  $$ |____  $$|_  $$_/   /$$__  $$
        | $$__  $$| $$  \ $$| $$| $$ \ $$ \ $$  /$$$$$$$  | $$    | $$$$$$$$
        | $$  | $$| $$  | $$| $$| $$ | $$ | $$ /$$__  $$  | $$ /$$| $$_____/
        | $$  | $$| $$  | $$| $$| $$ | $$ | $$|  $$$$$$$  |  $$$$/|  $$$$$$$
        |__/  |__/|__/  |__/|__/|__/ |__/ |__/ \_______/   \___/   \______*/
        
        //Animate
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
    }

    private void StopMovement() {
        isMoving = false;
        isRunning = false;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
    }

    //Health
    public override bool Damage(float amount) {
        //Damage
        bool success = base.Damage(amount);

        //Died -> Open death menu
        if (success && !IsAlive) MenuManager.Open(MenusList.Death);

        //Return success
        return success;
    }

    //Events (other)
    private void OnMenuChanged(string oldMenu, string newMenu) {
        //Change POV depending on menu
        switch (newMenu) {
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

    private void OnMenuTransitionStart(string oldMenu, string newMenu) {
        //Menu transition started -> Stop player
        StopMovement();
    }

}
