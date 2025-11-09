using UnityEngine;

public class BeastChargeState : EnemyState {

    //Constructor
    public BeastChargeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Gather info
        CapsuleCollider capsule = Enemy.Collider as CapsuleCollider;
        Transform transform = Enemy.transform;
        Vector3 capsuleStart = transform.position;
        Vector3 capsuleEnd = transform.position + capsule.height * Vector3.up;
        Vector3 forward = transform.forward;
        float maxMoveDistance = 10f;

        //Calculate max movement forward
        bool hit = Physics.CapsuleCast(capsuleStart, capsuleEnd, capsule.radius, forward, out RaycastHit hitInfo, maxMoveDistance + capsule.radius, LayerMask.GetMask("Default"));

        Vector3 movePosition = hit ? 
            capsuleStart + (hitInfo.distance - capsule.radius) * forward : //Hit something -> Move right before the hit
            capsuleStart + maxMoveDistance * forward; //Didn't hit nothing -> Max distance possible
    }

    public override void OnExit() {
        //Called when the state exits
    }

    public override void Execute() {
        //State logic loop
    }

}
