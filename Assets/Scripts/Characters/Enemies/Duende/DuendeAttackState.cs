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

        //Start coroutine
        attackCoroutine = Enemy.StartCoroutine(AttackCoroutine());
    }

    public override void OnExit()
    {
        //Stop coroutine
        if (attackCoroutine != null) Enemy.StopCoroutine(attackCoroutine);
    }

    //Attack
    private IEnumerator AttackCoroutine() {
        //Animate
        Enemy.Animator.SetTrigger("Attack");
        Duende.ThrowSpear();
        
        //Wait
        yield return new WaitForSeconds(Duende.spearCoolDown);

        if (Enemy.PlayerDistance < Duende.evadeRange)
        {
            //Player too close - > evade
            Duende.SetState(new DuendeEvadeState(Duende));
        }else if(Enemy.PlayerDistance < Duende.maxAttackRange)
        {
            //Aun en rango -> Sigue atacando
            attackCoroutine = Enemy.StartCoroutine(AttackCoroutine());
        }
        else
        {
            //Player fuera de rango -> siguele
            Duende.SetState(new DuendeFollowState(Duende));
        }
        
    }

}
