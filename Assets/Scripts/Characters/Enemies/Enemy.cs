using System.Collections;
using Botpa;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character {

    //Divider
    [Header("________________"), Space(10)]

    //Enemy
    [Header("Enemy")]
    [SerializeField] private bool _isBoss;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;

    public bool IsEnabled { get; private set; } = true;

    public Level Level { get; private set; }
    public Player Player { get; private set; }
    public EnemyBehaviour Behaviour { get; private set; }

    public bool IsBoss => _isBoss;
    public NavMeshAgent Agent => _agent;
    public Collider Collider => _collider;
    public Rigidbody Rigidbody => _rigidbody;

    //Movement & Rotation
    [Header("Movement & Rotation")]
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float rotateSpeed = 500;
    [SerializeField, Range(0, 1)] private float pushForceResistance = 0;

    public bool UseAutomaticRotation { get; private set; } = true;

    public bool AgentReachedDestination => Agent.hasPath && !Agent.pathPending && Agent.remainingDistance <= 0.1f;

    public override Vector3 MoveVelocity => Agent.desiredVelocity;

    //Targets
    [Header("Targets")]
    [SerializeField] private float viewDistance = 5;

    public Character Target { get; private set; }
    public bool TargetIsVisible { get; private set; }
    public bool TargetPositionIsKnown { get; private set; }
    public float TargetLastKnownDistance { get; private set; }
    public Vector3 TargetLastKnownPosition { get; private set; }

    //Room
    public Room Room { get; private set; } = null;

    //Helpers
    private const float GET_POINT_ERROR = 0.1f;


    //State
    protected override void OnStart() {
        //Get behaviour
        Behaviour = GetComponent<EnemyBehaviour>();
    
        //Get level & player references
        Level = Game.Current.Level;
        Player = Level.Player;

        //Take player as target
        Target = Player;

        //Heal to max health
        Heal(HealthMax);

        //Update effects
        AddOnEffectsUpdated(OnEffectsUpdated);
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
            if (!lookDirection.IsEmpty()) Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.LookRotation(lookDirection.normalized, Vector3.up), Time.deltaTime * rotateSpeed);
        }
    }

    public void SetEnabled(bool enabled) {
        //Enable
        IsEnabled = enabled;
        Agent.enabled = enabled;
        Collider.enabled = enabled && IsAlive;
    }

    public void NotifyPlayerEnteredRoom() {
        //Enable enemy
        SetEnabled(true);

        //Assign as boss if needed
        if (enabled && IsBoss && Game.Current.MenuManager.TryGetMenu(out GameMenu menu)) menu.AssignBoss(this);

        //Call event
        Behaviour.OnPlayerEnteredRoom();
    }

    //Health
    private IEnumerator DestroyCoroutine(float percent = 0) {
        while (percent < 1) {
            //Update percent
            percent = Mathf.Clamp01(percent + (Time.deltaTime / 3.0f)); //3 seconds
            Renderer.material.SetFloat("_Disintegration", percent);
            
            //Wait
            yield return new WaitForNextFrameUnit();
        }

        //Destroy enemy object
        Destroy(gameObject);
    }

    protected override void OnDamageFeedbackStart(DamageType type) {
        //Slow time
        if (type != DamageType.Burn) Game.SlowTime(this);

        //Update color
        if (Renderer) Renderer.material.SetColor("_Color", Color.red);
    }

    protected override void OnDamageFeedbackEnd(DamageType type) {
        //Stop slowing time
        if (type != DamageType.Burn) Game.UnslowTime(this);

        //Update color
        if (Renderer) Renderer.material.SetColor("_Color", Color.white);
    }

    protected override void OnDeath() {
        base.OnDeath();

        //Notify room that enemy was killed
        if (Room) Room.EnemyKilled(this);

        //Stop moving
        StopMovement();

        //Disable
        Rigidbody.linearVelocity = Vector3.zero; //Stop push forces
        Collider.enabled = false;
        Agent.enabled = false;
        enabled = false;

        //Call behaviour event
        Behaviour.OnDeath();

        //Start destroy countdown
        Level.StartCoroutine(DestroyCoroutine()); //Start coroutine in level cause its always active (when a gameobject, in this case the room, gets disabled, it stops all child coroutines)
    }

    public override bool Damage(float amount, DamageType type, object source) {
        //Check if damage is allowed
        amount = Behaviour.OnBeforeDamage(amount, type, source);

        //Damage
        bool damaged = base.Damage(amount, type, source);

        //Check if damaged
        if (damaged) {
            //Damaged -> Call behaviour event
            Behaviour.OnDamage(type, source);
        }

        //Return if damaged
        return damaged;
    }

    //Movement & Rotation
    public override void Push(Vector3 direction) {
        //Only allow pushing if alive
        if (!IsAlive) return;

        //Push
        Rigidbody.AddForce(Mathf.Clamp01(1 - pushForceResistance) * direction, ForceMode.Impulse);
    }

    public void StopMovement() {
        //Not on a navmesh
        if (!Agent.isOnNavMesh) return;

        //Stop movement
        Agent.ResetPath();
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

    public override void LookInDirection(Vector3 direction) {
        //Using automatic rotation -> Ignore
        if (UseAutomaticRotation) return;

        //Update rotation
        base.LookInDirection(direction);
    }

    public override void LookTowardsPoint(Vector3 point) {
        //Using automatic rotation -> Ignore
        if (UseAutomaticRotation) return;

        //Update rotation
        base.LookTowardsPoint(point);
    }

    //Targets
    private void CheckTargetVisible() {
        //Check if target is visible
        TargetIsVisible = Target.IsVisible(Eyes.position, viewDistance, LayerMask.GetMask("Default", "Player"));

        //Save distance
        TargetLastKnownDistance = Vector3.Distance(Target.transform.position, transform.position);

        //Save position if visible
        if (TargetIsVisible) {
            TargetPositionIsKnown = true;
            TargetLastKnownPosition = Target.transform.position;
        }
    }

    public void NotifyTargetPositionReached() {
        //Used for when the enemy reaches the last known position of the player
        TargetPositionIsKnown = TargetIsVisible;
    }

    //Effects
    private void OnEffectsUpdated() {
        //Update move speed
        Agent.speed = moveSpeed * SpeedMultiplier;
    }

    //Room
    public void SetRoom(Room room) {
        Room = room;
        SetEnabled(false); //Disable until room is entered
    }

    //Helpers
    public Enemy SpawnEnemy(GameObject prefab, Transform spawn) {
        if (Room) {
            //Spawn with room
            Enemy enemy = Room.InitializeEnemy(Instantiate(prefab, spawn.position, Quaternion.identity));
            enemy.LookInDirection(spawn.forward);
            enemy.SetEnabled(true);
            return enemy;
        } else {
            //Spawn without room
            return Instantiate(prefab, spawn.position, Quaternion.identity).GetComponent<Enemy>();
        }
    }

    public Vector3 GetFurthestPoint(Vector3 moveDirection, float maxDistance) {
        //Gather info
        CapsuleCollider capsule = Collider as CapsuleCollider;
        Vector3 capsuleStart = Model.position + GET_POINT_ERROR * Vector3.up;
        Vector3 capsuleEnd = Model.position + (capsule.height - 2 * GET_POINT_ERROR) * Vector3.up;
        float radius = capsule.radius - GET_POINT_ERROR;

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
