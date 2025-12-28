using System.Collections;
using System.Collections.Generic;
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

    public bool AgentReachedDestination => Agent.isOnNavMesh && !Agent.pathPending && Agent.remainingDistance <= 0.1f; // && Agent.hasPath

    public override Vector3 MoveVelocity => Agent.desiredVelocity;

    //Targets
    [Header("Targets")]
    [SerializeField] private float viewDistance = 5;

    public Character Target { get; private set; }
    public bool TargetIsVisible { get; private set; }               //If the target is visible
    public bool TargetPositionIsKnown { get; private set; }         //If the target was seen at some point and it's position was saved
    public Vector3 TargetLastKnownPosition { get; private set; }    //The last known target position
    public float TargetLastKnownDistance { get; private set; }      //The distance to the last known target position

    private const float MAX_NOTIFY_ENEMIES_DISTANCE = 4f;

    //Attack
    [Header("Attack")]
    [SerializeField, Min(0)] private float attackLevelFloorMultiplier = 0.5f;

    //Room
    public Room Room { get; private set; } = null;

    //Helpers
    private const float GET_POINT_ERROR = 0.1f;


    //State
    protected override void OnAwake() {
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
        UpdateTargetInfo();

        //Call behaviour event
        Behaviour.OnUpdate();

        //Rotate model
        if (UseAutomaticRotation) {
            Vector3 lookDirection = Util.RemoveY(TargetIsVisible ? (TargetLastKnownPosition - transform.position) : Agent.desiredVelocity);
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
    private void OnTargetPositionIsKnown(Vector3 position) {
        //Mark target position as known
        TargetPositionIsKnown = true;

        //Save target position & distance
        TargetLastKnownPosition = position;
        TargetLastKnownDistance = Vector3.Distance(TargetLastKnownPosition, transform.position);
    }

    private void UpdateTargetInfo() {
        //Check if target is visible
        TargetIsVisible = Target.IsVisible(Eyes.position, viewDistance, LayerMask.GetMask("Default", "Player"));

        //Check if target is visible
        if (TargetIsVisible) {
            //Is visible -> Save its position
            OnTargetPositionIsKnown(Target.transform.position);

            //Notify other enemies nearby
            foreach (var enemy in Room.Enemies) {
                //Check if enemy already knows target position
                if (enemy.TargetPositionIsKnown) continue;

                //Check if enemy is too far
                if (Vector3.Distance(transform.position, enemy.transform.position) > MAX_NOTIFY_ENEMIES_DISTANCE) continue;

                //Alert enemy that target position is known
                enemy.AlertTargetPositionIsKnown(TargetLastKnownPosition);
            }
        } else {
            //Not visible -> Update distance to last known position
            TargetLastKnownDistance = Vector3.Distance(TargetLastKnownPosition, transform.position);
        }
    }

    public void NotifyTargetPositionReached() {
        //Used for when the enemy reaches the last known position of the target
        TargetPositionIsKnown = TargetIsVisible;
    }

    public void AlertTargetPositionIsKnown(Vector3 position) {
        //Save target position
        OnTargetPositionIsKnown(position);

        //Do an animation or sum, idk
        Debug.Log("Enemy alerted, he now knows ball");
    }

    //Effects
    private void OnEffectsUpdated() {
        //Update move speed
        Agent.speed = moveSpeed * SpeedMultiplier;
    }

    //Attack
    public override float CalculateDamage(float damage) {
        return base.CalculateDamage(damage) * (1 + (Level.Floor - 1) * attackLevelFloorMultiplier);
    }

    //Room
    public void SetRoom(Room room) {
        Room = room;
        SetEnabled(false); //Disable until room is entered
    }

    //Helpers
    public Enemy SpawnEnemy(GameObject prefab, Transform spawn) {
        Enemy enemy = Room.InitializeEnemy(Instantiate(prefab, spawn.position, Quaternion.identity));
        enemy.LookInDirection(spawn.forward);
        enemy.SetEnabled(true);
        return enemy;
    }

    public Vector3 GetFurthestPoint(Vector3 moveDirection, float maxDistance) {
        //Gather info
        Vector3 position = Model.position;
        CapsuleCollider capsule = Collider as CapsuleCollider;
        Vector3 capsuleStart = position + GET_POINT_ERROR * Vector3.up;
        Vector3 capsuleEnd = position + (capsule.height - 2 * GET_POINT_ERROR) * Vector3.up;
        float radius = capsule.radius - GET_POINT_ERROR;

        //Check for max forward distance
        bool hit = Physics.CapsuleCast(capsuleStart, capsuleEnd, radius, moveDirection, out RaycastHit hitInfo, maxDistance + radius, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);

        //Return furthest point
        return hit ?
            //Hit something -> Move right before the hit
            position + (hitInfo.distance - radius) * moveDirection :
            //Didn't hit nothing -> Max distance possible
            position + maxDistance * moveDirection;
    }

    public (Vector3 point, float distance) GetFurthestPointAndDistance(Vector3 moveDirection, float maxDistance) {
        Vector3 point = GetFurthestPoint(moveDirection, maxDistance);
        float distance = Vector3.Distance(point, Model.position);
        return (point, distance);
    }

}
