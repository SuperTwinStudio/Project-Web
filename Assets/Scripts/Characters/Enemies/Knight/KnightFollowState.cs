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
            Knight.SetState(new KnightIdleState(Knight));
        } else if (Enemy.TargetLastKnownDistance <= Knight.AttackRange && Vector3.Angle(Enemy.Model.forward, (Enemy.TargetLastKnownPosition - Enemy.Model.position).normalized) < 10) {
            //Target in attack range & in front -> Attack it
            Knight.SetState(new KnightAttackState(Knight));
        } else {
            //Move towards target
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);

            //Check if reached destination
            if (Enemy.AgentReachedDestination) {
                //Notify target position was reached
                Enemy.NotifyTargetPositionReached();

                //Check if should stop following
                if (!Enemy.TargetIsVisible) Knight.SetState(new KnightIdleState(Knight));
            }
        }
    }

}
