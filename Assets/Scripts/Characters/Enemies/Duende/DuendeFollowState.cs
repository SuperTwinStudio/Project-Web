public class DuendeFollowState : DuendeState {

    //Constructor
    public DuendeFollowState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check target visibility
        if (!Enemy.TargetPositionIsKnown) {
            //Target position is unknown -> Go to idle
            Duende.SetState(new DuendeIdleState(Duende));
        } else if (Enemy.TargetIsVisible && Enemy.TargetLastKnownDistance > Duende.EvadeRange && Enemy.TargetLastKnownDistance < Duende.MinAttackRange) {
            //Target in attack range -> Attack it
            Duende.SetState(new DuendeAttackState(Duende));
        } else {
            //Move towards target
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);
    
            //Check if reached destination
            if (Enemy.AgentReachedDestination) {
                //Notify target position was reached
                Enemy.NotifyTargetPositionReached();

                //Check if should stop following
                if (!Enemy.TargetIsVisible) Behaviour.SetState(new DuendeIdleState(Behaviour));
            }
        }
    }

}
