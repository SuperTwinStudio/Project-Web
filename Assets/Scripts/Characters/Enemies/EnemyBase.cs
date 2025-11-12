using Botpa;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : Character {

    //Player
    public Player Player { get; private set; }

    //Components
    [Header("Components")]
    [SerializeField] private AttackHelper _attack;
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _model;
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _renderer;

    public bool IsEnabled { get; private set; } = true;
    public EnemyBehaviour Behaviour { get; private set; }

    public AttackHelper Attack => _attack;
    public Collider Collider => _collider;
    public Transform Model => _model;
    public Animator Animator => _animator;
    public Renderer Renderer => _renderer;

    //Health
    [Header("Health")]
    [SerializeField] private float _maxhealth = DEFAULT_HEALTH_MAX;
    [SerializeField] private GameObject damageIndicatorPrefab;

    public override float HealthMax => _maxhealth + EffectExtraHealth;

    //Movement
    [Header("Movement")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float viewDistance = 5;

    public bool PlayerIsVisible { get; private set; }
    public float PlayerDistance { get; private set; }
    public Vector3 PlayerLastKnownPosition { get; private set; }
    public bool UseAutomaticRotation { get; private set; } = true;

    public NavMeshAgent Agent => _agent;
    public Rigidbody Rigidbody => _rigidbody;

    //Room
    public Room Room { get; private set; } = null;


    //State
    private void Start() {
        //Check for missing behaviour
        Behaviour = GetComponent<EnemyBehaviour>();

        //Save player reference
        Player = Game.Current.Level.Player;

        //Heal to max health
        Heal(HealthMax);

        //Update effects
        OnEffectsUpdated();
    
        //Init behaviour
        Behaviour.Init(this);
    }

    protected override void OnUpdate() {
        //Not enabled
        if (!IsEnabled) return;

        //Check if player is visible
        CheckPlayerVisible();

        //Call behaviour event
        Behaviour.OnUpdate();

        //Rotate model
        if (UseAutomaticRotation) {
            Vector3 lookDirection = PlayerIsVisible ? (PlayerLastKnownPosition - transform.position) : Agent.desiredVelocity;
            if (!lookDirection.IsEmpty()) Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.LookRotation(lookDirection.normalized, Vector3.up), Time.deltaTime * 500);
        }
    }

    public void SetEnabled(bool enabled) {
        IsEnabled = enabled;
        Agent.enabled = enabled;
        Collider.enabled = enabled && IsAlive;
    }

    //Player
    private void CheckPlayerVisible() {
        //Check if player is visible
        PlayerIsVisible = Player.IsVisible(Eyes.position, viewDistance, LayerMask.GetMask("Default", "Player"));

        //Save distance
        PlayerDistance = Vector3.Distance(Player.transform.position, transform.position);

        //Save position if visible
        if (PlayerIsVisible) PlayerLastKnownPosition = Player.transform.position;
    }

    //Movement
    public override void Push(Vector3 direction) {
        Rigidbody.AddForce(direction, ForceMode.Impulse);
    }

    public void StopMovement() {
        //Not on a navmesh
        if (!Agent.isOnNavMesh) return;

        //Stop movement
        Agent.isStopped = true;
        Animator.SetBool("IsMoving", false);
    }

    public void MoveTowards(Vector3 position) {
        //Not on a navmesh
        if (!Agent.isOnNavMesh) return;

        //Move towards position
        Agent.isStopped = false;
        Agent.SetDestination(position);
        Animator.SetBool("IsMoving", true);
    }

    public void SetAutomaticRotation(bool automaticRotation) {
        UseAutomaticRotation = automaticRotation;
    }

    public void LookTowards(Vector3 position) {
        //Using automatic rotation -> Ignore
        if (UseAutomaticRotation) return;
        
        //Update rotation
        Model.rotation = Quaternion.LookRotation((position - Model.position).normalized);
    }

    //Health
    protected override void OnDamageFeedbackStart() {
        base.OnDamageFeedbackStart();

        //Update color
        if (Renderer) Renderer.material.SetColor("_Color", Color.red);
    }

    protected override void OnDamageFeedbackEnd() {
        base.OnDamageFeedbackEnd();

        //Update color
        if (Renderer) Renderer.material.SetColor("_Color", Color.white);
    }

    protected override void OnDeath() {
        //Notify room that enemy was killed
        if (Room) Room.EnemyKilled(this);

        //Stop moving
        StopMovement();

        //Disable collisions
        Collider.enabled = false;

        //Disable script
        enabled = false;

        //Call behaviour event
        Behaviour.OnDeath();
    }

    public override bool Damage(float amount, object source, DamageType type = DamageType.None) {
        //Check if damage is allowed
        amount = Behaviour.OnBeforeDamage(amount, source, type);

        //Damage
        bool damaged = base.Damage(amount, source, type);

        //Check if damaged
        if (damaged) {
            //Show damage indicator
            if (type != DamageType.Burn) Instantiate(damageIndicatorPrefab, Top.position + 0.3f * Vector3.up, Quaternion.identity).GetComponent<DamageTextIndicator>().SetDamage(amount *= EffectDamageTakenMultiplier, type);

            //Call behaviour event
            Behaviour.OnDamage();
        }

        //Return if damaged
        return damaged;
    }

    //Effects
    protected override void OnEffectsUpdated() {
        //Update move speed
        Agent.speed = moveSpeed * SpeedMultiplier;
    }

    //Attack
    public override float CalculateDamage(float damage) {
        return damage * Player.EffectDamageDealtMultiplier;
    }

    //Room
    public void SetRoom(Room room) {
        Room = room;
        SetEnabled(false); //Disable on creation
    }

}
