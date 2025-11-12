using System.Collections;
using UnityEngine;

public class LadronzueloStealState : LadronzueloState {

    //Steal
    private Coroutine stealCoroutine = null;


    //Constructor
    public LadronzueloStealState(EnemyBehaviour behaviour) : base(behaviour) { }

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
            Ladronzuelo.InteractRange / 2,
            0,
            0,
            (damageable) => Ladronzuelo.StealGoldFromPlayer()
        );

        //Wait
        yield return new WaitForSeconds(0.5f);

        //Return to aproach state
        if (!Ladronzuelo.CheckIfAllowedToSteal() && Ladronzuelo.StolenAmount > 0) {
            //Stole gold -> Flee from player
            Behaviour.SetState(new LadronzueloFleeState(Behaviour));
        } else {
            //keep following player
            Behaviour.SetState(new LadronzueloApproachState(Behaviour));   
        }
    }

}
