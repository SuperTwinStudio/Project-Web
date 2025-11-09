using UnityEngine;

public class BeastChargeState : BeastState {

    //Constructor
    public BeastChargeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Gather info
        CapsuleCollider capsule = Enemy.Collider as CapsuleCollider;
        Vector3 capsuleStart = Enemy.Model.position;
        Vector3 capsuleEnd = Enemy.Model.position + capsule.height * Vector3.up;
        Vector3 forward = Enemy.Model.forward;

        //Check for max forward distance
        bool hit = Physics.CapsuleCast(capsuleStart, capsuleEnd, capsule.radius, forward, out RaycastHit hitInfo, Beast.MaxChargeDistance + capsule.radius, LayerMask.GetMask("Default"));

        //Move to position
        Enemy.MoveTowards(hit ? 
            //Hit something -> Move right before the hit
            capsuleStart + (hitInfo.distance - capsule.radius) * forward : 
            //Didn't hit nothing -> Max distance possible
            capsuleStart + Beast.MaxChargeDistance * forward
        );
    }

    public override void Execute() {
        //Aura check
        base.Execute();

        //Does not have a path yet
        if (Enemy.Agent.pathPending) return;

        //Check distance
        if (Enemy.Agent.remainingDistance <= 0.1f) {
            //Reached destination -> Go to stunned state
            Enemy.StopMovement();
            Behaviour.SetState(new BeastStunState(Behaviour), true);
        } else {
            //Moving -> Update trigger
            
        }
    }

}
