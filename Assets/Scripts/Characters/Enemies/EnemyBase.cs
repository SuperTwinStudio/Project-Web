using Botpa;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : Character {

    //Player
    public Player Player { get; protected set; }

    //Components
    [Header("Components")]
    [SerializeField] protected EnemyBehaviour _behaviour;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected Transform _model;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Renderer _renderer;

    public EnemyBehaviour Behaviour => _behaviour;
    public Collider Collider => _collider;
    public Transform Model => _model;
    public Animator Animator => _animator;
    public Renderer Renderer => _renderer;

    //Movement
    [Header("Movement")]
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected float moveSpeed = 3;
    [SerializeField] protected float viewDistance = 5;

    public bool PlayerIsVisible { get; protected set; }
    public float PlayerDistance { get; protected set; }
    public Vector3 PlayerLastKnownPosition { get; protected set; }

    public NavMeshAgent Agent => _agent;
    public Rigidbody Rigidbody => _rigidbody;

    //Health
    [Header("Health")]
    [SerializeField] protected GameObject damageIndicatorPrefab;
    [SerializeField] protected float _maxHealth = DEFAULT_HEALTH_MAX;

    public override float HealthMax => _maxHealth;

    //Room
    public Room Room { get; protected set; } = null;


    //State
    protected virtual void Start() {
        //Save player reference
        Player = Game.Current.Level.Player;

        //Update effects
        OnEffectsUpdated();
    
        //Call behaviour event
        Behaviour.OnStart();
    }

    protected override void OnUpdate() {
        //Check if player is visible
        CheckPlayerVisible();

        //Call behaviour event
        Behaviour.OnUpdate();

        //Rotate model
        Vector3 lookDirection = Agent.desiredVelocity;
        if (!lookDirection.IsEmpty()) Model.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
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

    public void Push(Vector3 direction) {
        Rigidbody.AddForce(direction, ForceMode.Impulse);
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

    public override bool Damage(float amount, object source, DamageType type = DamageType.None) {
        //Update amount
        amount *= damageTakenMultiplier;

        //Damage
        bool damaged = base.Damage(amount, source, type);

        //Check if damaged
        if (damaged) {
            //Show damage indicator
            if (type != DamageType.Burn) Instantiate(damageIndicatorPrefab, Top.position + 0.3f * Vector3.up, Quaternion.identity).GetComponent<DamageTextIndicator>().SetDamage(amount, type);

            //Call behaviour event
            Behaviour.OnDamage();
        }

        //Return if damaged
        return damaged;
    }

    protected override void OnDeath() {
        //Call behaviour event
        Behaviour.OnDeath();
    }

    //Effects
    protected override void OnEffectsUpdated() {
        //Update move speed
        Agent.speed = moveSpeed * slowMovementMultiplier;
    }

    //Room
    public void SetRoom(Room room) {
        Room = room;
    }

}
