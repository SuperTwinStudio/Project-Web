using UnityEngine;

public class BeastChargeState : BeastState {

    //Delta
    private const float ERROR_DELTA = 0.1f;


    //Constructor
    public BeastChargeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Get furthest point forward
        Vector3 point = Beast.GetFurthestPoint(Enemy.Model.forward, Beast.MaxChargeDistance);

        //Move to point
        Enemy.MoveTowards(point);
    }

    public override void Execute() {
        //Aura check
        base.Execute();

        //No path yet
        if (Enemy.Agent.pathPending) return;

        //Check distance
        if (Enemy.Agent.remainingDistance <= 0.1f) {
            //Reached destination -> Go to stunned state
            Enemy.StopMovement();
            Behaviour.SetState(new BeastStunState(Behaviour), true);
        }
    }

}
