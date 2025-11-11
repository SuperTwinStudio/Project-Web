using System;
using Botpa;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : Character {

    //Player
    public Player Player { get; private set; }

    //Components
    [Header("Components")]
    [SerializeField] private EnemyBehaviour _behaviour;
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _model;
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _renderer;

    public bool IsEnabled { get; private set; } = true;

    public EnemyBehaviour Behaviour => _behaviour;
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
        if (!Behaviour) _behaviour = GetComponent<EnemyBehaviour>();

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
    }

    public void MoveTowards(Vector3 position) {
        //Not on a navmesh
        if (!Agent.isOnNavMesh) return;

        //Move towards position
        Agent.SetDestination(position);
        Agent.isStopped = false;
    }

    public void SetAutomaticRotation(bool automaticRotation) {
        UseAutomaticRotation = automaticRotation;
    }

    //Attack
    private bool DamageHits(RaycastHit[] hits, float damage, Action<IDamageable> onHit = null) {
        //Calculate damage
        damage = CalculateDamage(damage);

        //Bool to check if anything was hit
        bool somethingHit = false;

        //Check hits
        foreach (var hit in hits) {
            //Check if collision is a damageable
            if (!hit.collider.TryGetComponent(out IDamageable damageable)) continue;

            //Ignore all except player
            if (damageable is not global::Player) continue;

            //Damage
            if (damage > 0) damageable.Damage(damage, this, DamageType.Melee);

            //Mark as hit
            onHit?.Invoke(damageable);
            somethingHit = true;
        }

        //Return if anything was hit
        return somethingHit;
    }

    private float CalculateDamage(float damage) {
        return damage * Player.EffectDamageDealtMultiplier;
    }

    private RaycastHit[] AttackForwardCheck(float radius, float forward) {
        //Get forward direction
        Vector3 forwardDirection = Model.forward;

        //Casts a sphere of <radius> radius in front of the player and moves it forward <forward> amount to check for collisions
        return Physics.SphereCastAll(transform.position + radius * forwardDirection, radius, forwardDirection, forward);
    }

    private RaycastHit[] AttackAroundCheck(float radius) {
        //Casts a sphere of <radius> radius around the player
        return Physics.SphereCastAll(transform.position, radius, Vector3.up, 0);
    }

    public bool AttackForward(float radius, float forward, float damage, Action<IDamageable> onHit = null) {
        return DamageHits(AttackForwardCheck(radius, forward), damage, onHit);
    }

    public bool AttackAround(float radius, float damage, Action<IDamageable> onHit = null) {
        return DamageHits(AttackAroundCheck(radius), damage, onHit);
    }
    
    public Projectile SpawnProjectile(GameObject prefab, float damage, Vector3 direction, Transform origin = null) {
        Projectile projectile = Instantiate(prefab, (origin ? origin : Eyes).position, Quaternion.LookRotation(direction)).GetComponent<Projectile>();
        projectile.Init(this, CalculateDamage(damage));
        return projectile;
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

    //Room
    public void SetRoom(Room room) {
        Room = room;
        SetEnabled(false); //Disable on creation
    }

}
