using UnityEngine;

public class DuendeIdleState : DuendeState {

    //Constructor
    public DuendeIdleState(EnemyBehaviour Duende) : base(Duende) { }

    //Actions
    public override void OnEnter() {
        //Called when the state enters
    }

    public override void OnExit() {
        //Called when the state exits
    }

    public override void Execute() {
        //Check if player is visible
        if (!Enemy.PlayerIsVisible) return;

        if (Enemy.PlayerDistance > Duende.minAttackRange)
        {
            //Player too far -> follow him
            Duende.SetState(new DuendeFollowState(Duende));
        }
        else if (Enemy.PlayerDistance > Duende.evadeRange)
        {
            //Player within attack range -> Attack him
            Duende.SetState(new DuendeAttackState(Duende));
        }
        else
        {
            //Player too close -> Evade
            Duende.SetState(new DuendeEvadeState(Duende));
        }
        
    }

}
