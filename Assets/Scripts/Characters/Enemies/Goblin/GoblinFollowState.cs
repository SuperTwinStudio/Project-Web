public class GoblinFollowState : GoblinState {

    //Constructor
    public GoblinFollowState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check target visibility
        if (!Enemy.TargetPositionIsKnown) {
            //Target position is unknown -> Go to idle
            Behaviour.SetState(new GoblinIdleState(Behaviour), false);
        } else if (Enemy.TargetIsVisible && Enemy.TargetLastKnownDistance > Goblin.EvadeRange && Enemy.TargetLastKnownDistance < Goblin.MinAttackRange) {
            //Target in attack range -> Attack it
            Behaviour.SetState(new GoblinAttackState(Behaviour));
        } else {
            //Move towards target
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);
    
            //Check if reached destination
            if (Enemy.AgentReachedDestination) {
                //Notify target position was reached
                Enemy.NotifyTargetPositionReached();

                //Check if should stop following
                if (!Enemy.TargetIsVisible) Behaviour.SetState(new GoblinIdleState(Behaviour), false);
            }
        }
    }

}
