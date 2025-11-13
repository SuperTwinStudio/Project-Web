public class LadronzueloApproachState : LadronzueloState {

    //Constructor
    public LadronzueloApproachState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check target visibility
        if (!Enemy.TargetPositionIsKnown) {
            //Target position is unknown -> Go to idle
            Behaviour.SetState(new LadronzueloIdleState(Behaviour));
        } else if (Enemy.TargetLastKnownDistance <= Ladronzuelo.InteractRange) {
            //Target in interact range -> Check if allowed to steal
            if (Ladronzuelo.CheckIfAllowedToSteal()) {
                //Allowed to steal -> Steal
                Behaviour.SetState(new LadronzueloStealState(Behaviour), true);
            } else if (Ladronzuelo.HasStolen) {
                //Has stolen already -> Flee
                Behaviour.SetState(new LadronzueloFleeState(Behaviour));
            } else {
                //Stole nothin -> Attack
                Behaviour.SetState(new LadronzueloAttackState(Behaviour), true);
            }
        } else {
            //Target too far -> Move towards it
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);

            //Check if reached destination
            if (Enemy.AgentReachedDestination) {
                //Notify target position was reached
                Enemy.NotifyTargetPositionReached();

                //Check if should stop following
                if (!Enemy.TargetIsVisible) Behaviour.SetState(new LadronzueloIdleState(Behaviour));
            }
        }
    }

}
