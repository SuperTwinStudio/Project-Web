using System.Collections;
using UnityEngine;

public class BeastStunState : BeastState {

    //Stun
    private Coroutine coroutine = null;


    //Constructor
    public BeastStunState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Make enemy vulnerable
        Enemy.IsInvulnerable = false;

        //Start coroutine
        coroutine = Enemy.StartCoroutine(StunCoroutine());
    }

    public override void OnExit() {
        //Make enemy invulnerable
        Enemy.IsInvulnerable = true;

        //Stop coroutine
        if (coroutine != null) Enemy.StopCoroutine(coroutine);
    }

    //Stun
    private IEnumerator StunCoroutine() {
        //Animate
        Debug.Log("AMAI NO ME PEGUES QUE INDEFENSO ESTOY");

        //Wait
        yield return new WaitForSeconds(Beast.StunDuration);

        //Go start charging
        Behaviour.SetState(new BeastPrechargeState(Behaviour), true);
    }

}
