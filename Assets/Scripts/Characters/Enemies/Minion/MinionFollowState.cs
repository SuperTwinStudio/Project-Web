public class MinionFollowState : MinionState {

    //Constructor
    public MinionFollowState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check target visibility
        if (!Enemy.TargetPositionIsKnown) {
            //Target position is unknown -> Go to idle
            Behaviour.SetState(new MinionIdleState(Behaviour));
        } else if (Enemy.TargetLastKnownDistance <= Minion.AttackRange) {
            //Target in attack range -> Attack it
            Behaviour.SetState(new MinionAttackState(Behaviour), true);
        } else {
            //Target too far -> Move towards it
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);

            //Check if reached destination
            if (Enemy.AgentReachedDestination) {
                //Notify target position was reached
                Enemy.NotifyTargetPositionReached();

                //Check if should stop following
                if (!Enemy.TargetIsVisible) Behaviour.SetState(new MinionIdleState(Behaviour));
            }
        }
    }

}
