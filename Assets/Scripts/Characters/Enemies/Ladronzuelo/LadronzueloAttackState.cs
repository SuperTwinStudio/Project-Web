using System.Collections;
using UnityEngine;

public class LadronzueloAttackState : LadronzueloState {

    //Attack
    private Coroutine attackCoroutine = null;


    //Constructor
    public LadronzueloAttackState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnEnter() {
        //Start coroutine
        attackCoroutine = Enemy.StartCoroutine(AttackCoroutine());
    }

    public override void OnExit() {
        //Stop coroutine
        if (attackCoroutine != null) Enemy.StopCoroutine(attackCoroutine);
    }

    //Attack
    private IEnumerator AttackCoroutine() {
        //Animate
        Enemy.Animator.SetTrigger("Attack");

        //Wait
        yield return new WaitForSeconds(0.5f);

        //Attack
        Enemy.Attack.Forward(0.5f, 0, Ladronzuelo.AttackDamage);

        //Wait
        yield return new WaitForSeconds(0.5f);

        //Return to aproach state
        Behaviour.SetState(new LadronzueloApproachState(Behaviour));
    }

}
