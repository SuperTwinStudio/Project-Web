using System.Collections;
using UnityEngine;

public class ThiefStealState : ThiefState {

    //Steal
    private Coroutine stealCoroutine = null;


    //Constructor
    public ThiefStealState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnEnter() {
        //Start coroutine
        stealCoroutine = Enemy.StartCoroutine(StealCoroutine());
    }

    public override void OnExit() {
        //Stop coroutine
        if (stealCoroutine != null) Enemy.StopCoroutine(stealCoroutine);
    }

    //Steal
    private IEnumerator StealCoroutine() {
        //Animate
        Enemy.Animator.SetTrigger("Attack");

        //Wait
        yield return new WaitForSeconds(0.5f);

        //Steal
        Enemy.Attack.Forward(
            Thief.InteractRange / 2,
            0,
            0,
            true,
            (damageable) => Thief.StealGoldFromPlayer()
        );
    
        //Play sound
        Enemy.PlaySound(Thief.AttackSound);

        //Wait
        yield return new WaitForSeconds(0.5f);

        //Check next state
        if (Thief.HasStolen) {
            //Stole gold -> Go to flee
            Behaviour.SetState(new ThiefFleeState(Behaviour));
        } else {
            //Missed steal attack -> Go to aproach
            Behaviour.SetState(new ThiefFollowState(Behaviour));
        }
    }

}
