using System.Collections;
using UnityEngine;

public class BeastPrechargeState : BeastState {

    //Rage
    private Coroutine coroutine = null;


    //Constructor
    public BeastPrechargeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Enable automatic rotation
        Enemy.SetAutomaticRotation(true);

        //Start coroutine
        coroutine = Enemy.StartCoroutine(PrechargeCoroutine());
    }

    public override void OnExit() {
        //Disable automatic rotation
        Enemy.SetAutomaticRotation(false);

        //Stop coroutine
        if (coroutine != null) Enemy.StopCoroutine(coroutine);
    }

    //Charge
    private IEnumerator PrechargeCoroutine() {
        //Animate
        Debug.Log("LISTOS O NO, AYA VOY");

        //Wait
        yield return new WaitForSeconds(Beast.PrechargeDuration);

        //Go start charging
        Behaviour.SetState(new BeastChargeState(Behaviour), true);
    }

}
