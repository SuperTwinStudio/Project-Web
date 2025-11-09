using UnityEngine;

public class BeastChargeState : EnemyState {

    //Components
    private Transform transform;

    //Charge
    private Vector3 movePosition;
    private const float SPEED = 5.0f;


    //Constructor
    public BeastChargeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Gather info
        transform = Enemy.transform;
        CapsuleCollider capsule = Enemy.Collider as CapsuleCollider;
        Vector3 capsuleStart = transform.position;
        Vector3 capsuleEnd = transform.position + capsule.height * Vector3.up;
        Vector3 forward = transform.forward;
        float maxMoveDistance = 10f;

        //Check for max forward distance
        bool hit = Physics.CapsuleCast(capsuleStart, capsuleEnd, capsule.radius, forward, out RaycastHit hitInfo, maxMoveDistance + capsule.radius, LayerMask.GetMask("Default"));

        //Calculate move point
        movePosition = hit ? 
            capsuleStart + (hitInfo.distance - capsule.radius) * forward : //Hit something -> Move right before the hit
            capsuleStart + maxMoveDistance * forward; //Didn't hit nothing -> Max distance possible
    }

    public override void Execute() {
        //Move towards point
        Vector3 newPosition = Vector3.MoveTowards(transform.position, movePosition, Time.deltaTime * SPEED);
        Enemy.TeleportTo(newPosition);

        //Check if reached end
        if (newPosition == movePosition) Behaviour.SetState(new BeastStunState(Behaviour), true);
    }

}
