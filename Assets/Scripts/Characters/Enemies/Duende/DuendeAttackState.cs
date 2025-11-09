using System.Collections;
using UnityEngine;

public class DuendeAttackState : EnemyState {

    //Attack
    private const float ATTACK_DAMAGE = 20;
    private const float ATTACK_COOLDOWN = 0.5f;
    private Coroutine attackCoroutine = null;


    //Constructor
    public DuendeAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter()
    {
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

        //TODO: Throw the spear
        
        //Wait
        yield return new WaitForSeconds(ATTACK_COOLDOWN);

        //Return to idle state & execute it
        Behaviour.SetState(new DuendeIdleState(Behaviour), true);
    }

}
