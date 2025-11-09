using UnityEngine;

public class DuendeEvadeState : EnemyState
{

    //Attack
    private const float EVADE_RANGE = 1.5f;

    public DuendeEvadeState(EnemyBehaviour behaviour) : base(behaviour) {}
    
    //Actions
    public override void OnEnter() {
        //Called when the state enters
    }

    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
        Enemy.Animator.SetBool("IsMoving", false);
    }

    public override void Execute() {
        //Check player visibility
        if (!Enemy.PlayerIsVisible) {
            //Player not visible -> Go to idle
            Behaviour.SetState(new SimpleIdleState(Behaviour));
        } else if (Enemy.PlayerDistance <= EVADE_RANGE) {
            //Player in attack range -> Attack it
            Behaviour.SetState(new SimpleAttackState(Behaviour), true);
        } else {
            //Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
            Enemy.Animator.SetBool("IsMoving", true);
        }
    }
}
