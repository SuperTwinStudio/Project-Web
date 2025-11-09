using UnityEngine;

public class KnightIdleState : EnemyState
{

    //Constructor
    public KnightIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Called when the state enters
    }

    public override void OnExit() {
        //Called when the state exits
    }

    public override void Execute() {
        //Check if player is visible

        if(Enemy.PlayerDistance > 5) return;

        //Go to follow state & execute it
        Behaviour.SetState(new KnightUnsheatheShield(Behaviour), true);
    }
}
