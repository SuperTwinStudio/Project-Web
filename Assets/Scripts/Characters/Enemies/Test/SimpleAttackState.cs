using System.Collections;
using UnityEngine;

public class SimpleAttackState : EnemyState {

    //Attack
    private const float ATTACK_DAMAGE = 20;
    private Coroutine attackCoroutine = null;


    //Constructor
    public SimpleAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

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
        Enemy.AttackForward(0.75f, 0, ATTACK_DAMAGE);
    
        //Wait
        yield return new WaitForSeconds(0.5f);

        //Return to idle state & execute it
        Behaviour.SetState(new SimpleIdleState(Behaviour), true);
    }

}
