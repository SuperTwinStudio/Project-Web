using System;
using UnityEngine;

public class DuendeFollowState : EnemyState
{

    //Attack
    private const float ATTACK_RANGE = 5f;

    public DuendeFollowState(EnemyBehaviour behaviour) : base(behaviour) {}
    
    //Actions
    public override void OnEnter() {
        //Called when the state enters
    }

    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
        Enemy.Animator.SetBool("IsMoving", false);
    }

    public override void Execute()
    {
        //Check player visibility
        if (!Enemy.PlayerIsVisible)
        {
            //Player not visible -> Go to idle
            Behaviour.SetState(new DuendeIdleState(Behaviour));
        }
        else if (Enemy.PlayerDistance <= ATTACK_RANGE)
        {
            //Player in attack range -> Attack it
            Enemy.StopMovement();
            
        }
        else
        {
            //Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
            Enemy.Animator.SetBool("IsMoving", true);
        }
    }
}
