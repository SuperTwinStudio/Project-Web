using System.Collections;
using UnityEngine;

public class KnightAttackState : KnightState {

    //Attack
    private Coroutine coroutine = null;


    //Constructor
    public KnightAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Hide shield
        Knight.ToggleShield(false);

        //Disable autorotation
        Enemy.SetAutomaticRotation(false);

        //Animate attack
        Enemy.Animator.SetTrigger("Attack");
    }

    public override void OnExit() {
        //Reenable autorotation
        Enemy.SetAutomaticRotation(true);
    }

}
