using UnityEngine;

public class KnightUnsheatheShield : EnemyState
{

    //Constructor
    public KnightUnsheatheShield(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Called when the state enters
        Enemy.IsInvulnerable = true;
    }

    public override void OnExit() {
        //Called when the state exits
    }

    public override void Execute() {
        Debug.Log("Knight Unsheathing Shield");

        Behaviour.SetState(new KnightFollowState(Behaviour), true);

    }

}
