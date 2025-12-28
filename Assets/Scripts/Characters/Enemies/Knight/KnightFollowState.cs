using UnityEngine;

public class KnightFollowState : KnightState {

    //Constructor
    public KnightFollowState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Show shield
        Knight.ToggleShield(true);
    }

    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check target visibility
        if (!Enemy.TargetPositionIsKnown) {
            //Target position is unknown -> Go to idle
            Behaviour.SetState(new KnightIdleState(Behaviour), false);
        } else if (Enemy.TargetIsVisible && Enemy.TargetLastKnownDistance <= Knight.AttackRange && Vector3.Angle(Enemy.Model.forward, (Enemy.TargetLastKnownPosition - Enemy.Model.position).normalized) < 10) {
            //Target in attack range & in front -> Attack it
            Behaviour.SetState(new KnightAttackState(Behaviour));
        } else {
            //Move towards target
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);

            //Check if reached destination
            if (Enemy.AgentReachedDestination) {
                //Notify target position was reached
                Enemy.NotifyTargetPositionReached();

                //Check if should stop following
                if (!Enemy.TargetIsVisible) Behaviour.SetState(new KnightIdleState(Behaviour), false);
            }
        }
    }

}
