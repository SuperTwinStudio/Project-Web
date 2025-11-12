using UnityEngine;

public class KnightFollowState : EnemyState
{
    //Attack
    private const float ATTACK_RANGE = 1.5f;


    //Constructor
    public KnightFollowState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Called when the state enters
    }

    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check player visibility
        if (!Enemy.PlayerIsVisible) {
            //Player not visible -> Go to idle
            Behaviour.SetState(new KnightIdleState(Behaviour));
        } else if (Enemy.PlayerDistance <= ATTACK_RANGE) {
            //Player in attack range -> Attack it
            Behaviour.SetState(new KnightSheatheState(Behaviour), true);
        } else {
            //Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
        }
    }

}
