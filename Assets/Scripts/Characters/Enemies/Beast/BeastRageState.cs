using System.Collections;
using UnityEngine;

public class BeastRageState : BeastState {

    //Rage
    private Coroutine coroutine = null;


    //Constructor
    public BeastRageState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Start coroutine
        coroutine = Enemy.StartCoroutine(RageCoroutine());
    }

    public override void OnExit() {
        //Update walkable surface
        Game.Current.Level.UpdateWalkableSurface();

        //Stop coroutine
        if (coroutine != null) Enemy.StopCoroutine(coroutine);
    }

    //Rage
    private IEnumerator RageCoroutine() {
        //Shake screen
        Enemy.Player.CameraController.AddShake(Beast.RageDuration);

        //Animate
        Debug.Log("ESTOY TO ENFADAOO");

        //Wait
        yield return new WaitForSeconds(Beast.RageDuration);

        //Go start charging
        Behaviour.SetState(new BeastPrechargeState(Behaviour), true);
    }

}
