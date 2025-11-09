using System.Collections;
using UnityEngine;

public class BeastDeathState : BeastState {

    //Death
    private Coroutine coroutine = null;


    //Constructor
    public BeastDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Start coroutine
        coroutine = Enemy.StartCoroutine(DeathCoroutine());
    }

    public override void OnExit() {
        //Stop coroutine
        if (coroutine != null) Enemy.StopCoroutine(coroutine);
    }

    //Death
    private IEnumerator DeathCoroutine() {
        //Animate
        Enemy.Animator.SetTrigger("Die");
    
        //Wait
        yield return new WaitForSeconds(5);

        //Destroy enemy object
        GameObject.Destroy(Enemy.gameObject);
    }

}
