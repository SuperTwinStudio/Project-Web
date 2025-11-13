using System.Collections;
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
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Transform _model;
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _renderer;

    public bool IsEnabled { get; private set; } = true;
    public EnemyBehaviour Behaviour { get; private set; }

    public AttackHelper Attack => _attack;
    public Collider Collider => _collider;
    public AudioSource Audio => _audio;
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

    public bool TargetIsVisible { get; private set; }
    public bool TargetPositionIsKnown { get; private set; }
    public float TargetLastKnownDistance { get; private set; }
    public Vector3 TargetLastKnownPosition { get; private set; }

    public NavMeshAgent Agent => _agent;
    public Rigidbody Rigidbody => _rigidbody;

    public bool AgentReachedDestination => Agent.hasPath && !Agent.pathPending && Agent.remainingDistance <= 0.1f;

    //Rotation
    public bool UseAutomaticRotation { get; private set; } = true;

    //Room
    public Room Room { get; private set; } = null;

    //Helpers
    private const float ERROR_DELTA = 0.1f;


    //State
    private void Start() {
        //Get behaviour
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

        //Check if target is visible
        CheckTargetVisible();

        //Call behaviour event
        Behaviour.OnUpdate();

        //Rotate model
        if (UseAutomaticRotation) {
            Vector3 lookDirection = TargetIsVisible ? (TargetLastKnownPosition - transform.position) : Agent.desiredVelocity;
            if (!lookDirection.IsEmpty()) Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.LookRotation(lookDirection.normalized, Vector3.up), Time.deltaTime * 500);
        }
    }

    public void SetEnabled(bool enabled) {
        IsEnabled = enabled;
        Agent.enabled = enabled;
        Collider.enabled = enabled && IsAlive;
    }

    //Player
    private void CheckTargetVisible() {
        //Check if target is visible
        TargetIsVisible = Player.IsVisible(Eyes.position, viewDistance, LayerMask.GetMask("Default", "Player"));

        //Save distance
        TargetLastKnownDistance = Vector3.Distance(Player.transform.position, transform.position);

        //Save position if visible
        if (TargetIsVisible) {
            TargetPositionIsKnown = true;
            TargetLastKnownPosition = Player.transform.position;
        }
    }

    public void NotifyTargetPositionReached() {
        //Used for when the enemy reaches the last known position of the player
        TargetPositionIsKnown = TargetIsVisible;
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

    //Rotation
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
    private IEnumerator DeathCoroutine() {
        //Wait
        yield return new WaitForSeconds(5.0f);

        //Destroy enemy object
        Destroy(gameObject);
    }

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

        //Disable
        Collider.enabled = false;
        Agent.enabled = false;
        enabled = false;

        //Call behaviour event
        Behaviour.OnDeath();

        //Start destroy countdown
        StartCoroutine(DeathCoroutine());
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

    //Helpers
    public void PlaySound(AudioClip clip) {
        Audio.pitch = Random.Range(0.92f, 1.08f);
        Audio.PlayOneShot(clip);
    }

    public EnemyBase SpawnEnemy(GameObject prefab, Transform spawn) {
        if (Room) {
            //Spawn with room
            EnemyBase enemy = Room.InitializeEnemy(Instantiate(prefab, spawn.position, Quaternion.identity));
            enemy.SetEnabled(true);
            return enemy;
        } else {
            //Spawn without room
            return Instantiate(prefab, spawn.position, Quaternion.identity).GetComponent<EnemyBase>();
        }
    }

    public Vector3 GetFurthestPoint(Vector3 moveDirection, float maxDistance) {
        //Gather info
        CapsuleCollider capsule = Collider as CapsuleCollider;
        Vector3 capsuleStart = Model.position + ERROR_DELTA * Vector3.up;
        Vector3 capsuleEnd = Model.position + (capsule.height - 2 * ERROR_DELTA) * Vector3.up;
        float radius = capsule.radius - ERROR_DELTA;

        //Check for max forward distance
        bool hit = Physics.CapsuleCast(capsuleStart, capsuleEnd, radius, moveDirection, out RaycastHit hitInfo, maxDistance + radius, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);

        //Return furthest point
        return hit ?
            //Hit something -> Move right before the hit
            capsuleStart + (hitInfo.distance - radius) * moveDirection :
            //Didn't hit nothing -> Max distance possible
            capsuleStart + maxDistance * moveDirection;
    }

    public (Vector3 point, float distance) GetFurthestPointAndDistance(Vector3 moveDirection, float maxDistance) {
        Vector3 point = GetFurthestPoint(moveDirection, maxDistance);
        float distance = Vector3.Distance(point, Model.position);
        return (point, distance);
    }

}
