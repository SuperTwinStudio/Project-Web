using System.Collections;
using UnityEngine;

public class MinionAttackState : MinionState {

    //Attack
    private Coroutine attackCoroutine = null;


    //Constructor
    public MinionAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

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
        Enemy.Attack.Forward(0.75f, 0, Minion.AttackDamage);

        //Wait
        yield return new WaitForSeconds(0.5f);

        //Return to idle state & execute it
        Behaviour.SetState(new MinionIdleState(Behaviour), true);
    }

}
