using UnityEngine;

public class BeastChargeState : BeastState {

    //Constructor
    public BeastChargeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Play sound
        Enemy.PlaySound(Beast.ChargeSound);

        //Get furthest point forward
        Vector3 point = Enemy.GetFurthestPoint(Enemy.Model.forward, Beast.MaxChargeDistance);

        //Move to point
        Enemy.MoveTowards(point);
    }

    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Aura check
        base.Execute();

        //Check if reached destination
        if (Enemy.AgentReachedDestination) {
            //Reached destination -> Go to stunned state
            Behaviour.SetState(new BeastStunState(Behaviour), true);
        }
    }

}
