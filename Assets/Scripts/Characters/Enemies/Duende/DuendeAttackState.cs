using System.Collections;
using UnityEngine;

public class DuendeAttackState : DuendeState {

    //Attack
    private Coroutine attackCoroutine = null;


    //Constructor
    public DuendeAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter()
    {
        //Stop moving
        Enemy.StopMovement();

        //Attack
        Duende.Attack();
    }

    public override void OnExit()
    {
        //Stop coroutine
        if (attackCoroutine != null) Enemy.StopCoroutine(attackCoroutine);
    }

    //Attack
    public override void Execute()
    {
        if (Duende.onAttackCooldown) return;

        if (Enemy.PlayerDistance < Duende.evadeRange)
        {
            //Player too close - > evade
            Duende.SetState(new DuendeEvadeState(Duende));
        }else if(Enemy.PlayerDistance < Duende.maxAttackRange)
        {
            //Attack
            Duende.Attack();
        }
        else
        {
            //Player fuera de rango -> siguele
            Duende.SetState(new DuendeFollowState(Duende));
        }
    }

}
