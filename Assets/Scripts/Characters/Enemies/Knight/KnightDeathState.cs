using System.Collections;
using UnityEngine;

public class KnightDeathState : EnemyState {

    //Death
    private Coroutine deathCoroutine = null;


    //Constructor
    public KnightDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Start coroutine
        deathCoroutine = Enemy.StartCoroutine(DeathCoroutine());
    }

    public override void OnExit() {
        //Stop coroutine
        if (deathCoroutine != null) Enemy.StopCoroutine(deathCoroutine);
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
