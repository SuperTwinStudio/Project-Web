using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    //Components
    public EnemyBase Enemy { get; private set; }

    //State
    public EnemyState State { get; private set; }

    //Helpers
    private const float ERROR_DELTA = 0.1f;


    //Init
    public void Init(EnemyBase enemy) {
        //Save enemy
        Enemy = enemy;

        //Call init event
        OnInit();
    }

    protected virtual void OnInit() {
        //Init your state & any variables you need
    }

    //States
    public void SetState(EnemyState newState, bool execute = false) {
        //Check if same state
        if (State == newState) return;

        //Notify old state for exit changes
        State?.OnExit();

        //Update state
        State = newState;

        //Check new state for enter changes
        State?.OnEnter();

        //Execute state
        if (execute) State?.Execute();
    }

    //Update
    public void OnUpdate() {
        //Enemy logic loop
        State?.Execute();
    }

    //Health
    public virtual float OnBeforeDamage(float amount, object source, DamageType type = DamageType.None) {
        //Before the enemy was damaged
        return amount;
    }

    public virtual void OnDamage() {
        //Enemy was damaged
        State?.OnDamage();
    }

    public virtual void OnDeath() {
        //Stop moving
        Enemy.StopMovement();

        //Disable collisions
        Enemy.Collider.enabled = false;

        //Disable script
        Enemy.enabled = false;
    }

    //Helpers
    public Vector3 GetFurthestPoint(Vector3 moveDirection, float maxDistance) {
        //Gather info
        CapsuleCollider capsule = Enemy.Collider as CapsuleCollider;
        Vector3 capsuleStart = Enemy.Model.position + ERROR_DELTA * Vector3.up;
        Vector3 capsuleEnd = Enemy.Model.position + (capsule.height - 2 * ERROR_DELTA) * Vector3.up;
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
        float distance = Vector3.Distance(point, Enemy.Model.position);
        return (point, distance);
    }

    public EnemyBase SpawnEnemy(GameObject prefab, Transform spawn) {
        if (Enemy.Room) {
            //Spawn with room
            EnemyBase enemy = Enemy.Room.InitializeEnemy(Instantiate(prefab, spawn.position, Quaternion.identity));
            enemy.SetEnabled(true);
            return enemy;
        } else {
            //Spawn without room
            return Instantiate(prefab, spawn.position, Quaternion.identity).GetComponent<EnemyBase>();
        }
    }

}
