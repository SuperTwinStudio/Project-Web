using System.Collections;
using UnityEngine;

public class BeastRageState : EnemyState {

    //Rage
    private const float DURATION = 2.0f;
    private Coroutine coroutine = null;


    //Constructor
    public BeastRageState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Start coroutine
        coroutine = Enemy.StartCoroutine(RageCoroutine());
    }

    public override void OnExit() {
        //Stop coroutine
        if (coroutine != null) Enemy.StopCoroutine(coroutine);
    }

    //Rage
    private IEnumerator RageCoroutine() {
        //Shake screen
        Enemy.Player.CameraController.AddShake(DURATION);

        //Animate
        Debug.Log("ESTOY TO ENFADAOO");

        //Wait
        yield return new WaitForSeconds(DURATION);

        //Go start charging
        Behaviour.SetState(new BeastPrechargeState(Behaviour), true);
    }

}
