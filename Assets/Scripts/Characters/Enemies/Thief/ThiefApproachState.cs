public class ThiefApproachState : ThiefState {

    //Constructor
    public ThiefApproachState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check target visibility
        if (!Enemy.TargetPositionIsKnown) {
            //Target position is unknown -> Go to idle
            Behaviour.SetState(new ThiefIdleState(Behaviour));
        } else if (Enemy.TargetLastKnownDistance <= Thief.InteractRange) {
            //Target in interact range -> Check if allowed to steal
            if (Thief.CheckIfAllowedToSteal()) {
                //Allowed to steal -> Steal
                Behaviour.SetState(new ThiefStealState(Behaviour), true);
            } else {
                //Not allowed -> Attack
                Behaviour.SetState(new ThiefAttackState(Behaviour), true);
            }
        } else {
            //Target too far -> Move towards it
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);

            //Check if reached destination
            if (Enemy.AgentReachedDestination) {
                //Notify target position was reached
                Enemy.NotifyTargetPositionReached();

                //Check if should stop following
                if (!Enemy.TargetIsVisible) Behaviour.SetState(new ThiefIdleState(Behaviour));
            }
        }
    }

}
