using UnityEngine;

public class KnightSheatheState : EnemyState
{

    //Constructor
    public KnightSheatheState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Called when the state enters
        
    }

    public override void OnExit() {
        //Called when the state exits
        Enemy.IsInvulnerable = false;
    }

    public override void Execute() {
        Debug.Log("Knight Sheathing Shield");

        Behaviour.SetState(new KnightAttackState(Behaviour), true);

    }

}