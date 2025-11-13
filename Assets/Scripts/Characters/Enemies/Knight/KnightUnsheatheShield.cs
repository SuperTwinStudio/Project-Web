using UnityEngine;

public class KnightUnsheatheShield : KnightState {

    //Constructor
    public KnightUnsheatheShield(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Called when the state enters
        Enemy.IsInvulnerable = true;
    }

    public override void Execute() {
        Debug.Log("Knight Unsheathing Shield");
        Behaviour.SetState(new KnightFollowState(Behaviour), true);
    }

}
