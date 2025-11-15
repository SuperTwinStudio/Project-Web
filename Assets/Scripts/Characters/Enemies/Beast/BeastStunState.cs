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
        Beast.AuraModel.SetActive(false);

        //Start coroutine
        coroutine = Enemy.StartCoroutine(StunCoroutine());
    }

    public override void OnExit() {
        //Make enemy invulnerable (if still alive)
        if (Enemy.IsAlive) {
            Enemy.IsInvulnerable = true;
            Beast.AuraModel.SetActive(true);
        }

        //Stop coroutine
        if (coroutine != null) Enemy.StopCoroutine(coroutine);
    }

    //Stun
    private IEnumerator StunCoroutine() {
        //Animate
        Enemy.PlaySound(Beast.StunSound);

        //Wait
        yield return new WaitForSeconds(Beast.StunDuration);

        //Start charging
        Behaviour.SetState(new BeastPrechargeState(Behaviour), true);
    }

}
