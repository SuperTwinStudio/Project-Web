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
        Enemy.Attack.Forward(
            Minion.AttackRadius, 
            0, 
            Minion.AttackDamage
        );

        //Play sound
        Enemy.PlaySound(Minion.AttackSound);

        //Wait
        yield return new WaitForSeconds(0.5f);

        //Return to idle
        Behaviour.SetState(new MinionIdleState(Behaviour), false);
    }

}
