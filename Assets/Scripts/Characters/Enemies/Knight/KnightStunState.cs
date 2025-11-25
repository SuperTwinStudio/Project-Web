using System.Collections;
using UnityEngine;

public class KnightStunState : KnightState {

    //Stun
    private Coroutine coroutine = null;


    //Constructor
    public KnightStunState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Start coroutine
        coroutine = Enemy.StartCoroutine(StunCoroutine());
    }

    public override void OnExit() {
        //Stop coroutine
        if (coroutine != null) Enemy.StopCoroutine(coroutine);
    }

    //Stun
    private IEnumerator StunCoroutine() {
        //Wait
        yield return new WaitForSeconds(Knight.StunDuration);

        //Return to follow
        Knight.SetState(new KnightFollowState(Knight), true);
    }

}
