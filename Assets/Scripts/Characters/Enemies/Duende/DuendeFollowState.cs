using System;
using UnityEngine;

public class DuendeFollowState : DuendeState
{

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
            Duende.SetState(new DuendeIdleState(Duende));
        }
        else if (Enemy.PlayerDistance > Duende.evadeRange && Enemy.PlayerDistance < Duende.minAttackRange)
        {
            //Player in attack range -> Attack it
            Enemy.StopMovement();
            Duende.SetState(new DuendeAttackState(Duende));
        }
        else
        {
            //Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
            Enemy.Animator.SetBool("IsMoving", true);
        }
    }
}
