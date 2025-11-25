using System.Collections;
using UnityEngine;

public class KnightAttackState : KnightState {

    //Attack
    private Coroutine coroutine = null;


    //Constructor
    public KnightAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Hide shield
        Knight.ToggleShield(false);

        //Start coroutine
        coroutine = Enemy.StartCoroutine(AttackCoroutine());
    }

    public override void OnExit() {
        //Stop coroutine
        if (coroutine != null) Enemy.StopCoroutine(coroutine);
    }

    //Attack
    private IEnumerator AttackCoroutine() {
        //Animate
        Enemy.Animator.SetTrigger("Attack");
    
        //Wait
        yield return new WaitForSeconds(0.5f);

        //Attack
        bool hit = Enemy.Attack.Forward(Knight.AttackRadius, 0, Knight.AttackDamage);
    
        //Check if missed
        if (!hit) {
            //Missed -> Stun knight
            Knight.SetState(new KnightStunState(Knight));
        } else {
            //Wait
            yield return new WaitForSeconds(0.5f);
        
            //Show shield
            Knight.ToggleShield(true);
        
            //Wait
            yield return new WaitForSeconds(1f);

            //Return to follow
            Knight.SetState(new KnightFollowState(Knight), true);
        }
    }

}
