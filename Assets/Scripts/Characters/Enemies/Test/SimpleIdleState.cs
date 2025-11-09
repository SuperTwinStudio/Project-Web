public class SimpleIdleState : EnemyState {

    //Constructor
    public SimpleIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Called when the state enters
    }

    public override void OnExit() {
        //Called when the state exits
    }

    public override void Execute() {
        //Check if player is visible
        if (!Enemy.PlayerIsVisible) return;

        //Go to follow state & execute it
        Behaviour.SetState(new SimpleFollowState(Behaviour), true);
    }

}
