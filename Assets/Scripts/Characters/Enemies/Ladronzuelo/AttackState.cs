using System.Collections;
using UnityEngine;

public class AttackState : EnemyState {

    //Attack
    private const float ATTACK_DAMAGE = 20;
    private const float ATTACK_RANGE = 1.5f;
    private Coroutine attackCoroutine = null;


    //Constructor
    public AttackState(EnemyBehaviour behaviour) : base(behaviour) {}

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
        Enemy.AttackForward(0.5f, 0, ATTACK_DAMAGE);
    
        //Wait
        yield return new WaitForSeconds(0.5f);

        if(Enemy.PlayerDistance > ATTACK_RANGE)
        {
            Behaviour.SetState(new ApproachState(Behaviour), true);
        }
    }

}
