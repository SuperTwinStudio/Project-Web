using UnityEngine;

public class DuendeEvadeState : DuendeState
{
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
        if(Enemy.PlayerDistance > Duende.evadeRange)
        {
            //Player lejos -> mira a ver que haces
            Duende.SetState(new DuendeIdleState(Duende));
        }
        else
        {
            //Player cerca -> huye!
            Enemy.MoveTowards(Enemy.Bot.position + (Enemy.Eyes.position - Enemy.PlayerLastKnownPosition).normalized);
        }
    }
}
