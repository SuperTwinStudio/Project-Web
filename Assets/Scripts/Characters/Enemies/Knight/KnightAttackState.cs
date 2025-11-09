using System.Collections;
using UnityEngine;

public class KnightAttackState : EnemyState
{

    //Attack
    private const float ATTACK_DAMAGE = 20;
    private Coroutine attackCoroutine = null;


    //Constructor
    public KnightAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

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
        Debug.Log("Knight Attacking");

        //Animate
        Enemy.Animator.SetTrigger("Attack");
    
        //Wait
        yield return new WaitForSeconds(0.5f);

        //Attack
        Enemy.AttackForward(2f, 0, ATTACK_DAMAGE);
    
        //Wait
        yield return new WaitForSeconds(2f);

        Behaviour.SetState(new KnightFollowState(Behaviour), true);
    }

}
