public class ThiefFollowState : ThiefState {

    //Constructor
    public ThiefFollowState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check target visibility
        if (!Enemy.TargetPositionIsKnown) {
            //Target position is unknown -> Go to idle
            Behaviour.SetState(new ThiefIdleState(Behaviour), false);
        } else if (Enemy.TargetIsVisible && Enemy.TargetLastKnownDistance <= Thief.InteractRange) {
            //Target in interact range -> Check if can steal
            if (Thief.CanSteal) {
                //Can steal -> Steal
                Behaviour.SetState(new ThiefStealState(Behaviour));
            } else {
                //Can't steal -> Attack
                Behaviour.SetState(new ThiefAttackState(Behaviour));
            }
        } else {
            //Target too far -> Move towards it
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);

            //Check if reached destination
            if (Enemy.AgentReachedDestination) {
                //Notify target position was reached
                Enemy.NotifyTargetPositionReached();

                //Check if should stop following
                if (!Enemy.TargetIsVisible) Behaviour.SetState(new ThiefIdleState(Behaviour), false);
            }
        }
    }

}
