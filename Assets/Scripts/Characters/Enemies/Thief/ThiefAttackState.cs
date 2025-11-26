using System.Collections;
using UnityEngine;

public class ThiefAttackState : ThiefState {

    //Attack
    private Coroutine attackCoroutine = null;


    //Constructor
    public ThiefAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

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
            Thief.InteractRange / 2,
            0,
            Thief.AttackDamage
        );
    
        //Play sound
        Enemy.PlaySound(Thief.AttackSound);

        //Wait
        yield return new WaitForSeconds(0.5f);

        //Go to aproach
        Behaviour.SetState(new ThiefApproachState(Behaviour));
    }

}
