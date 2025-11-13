public class MinionIdleState : MinionState {

    //Constructor
    public MinionIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void Execute() {
        //Check if target is visible
        if (!Enemy.TargetIsVisible) return;

        //Follow target
        Behaviour.SetState(new MinionFollowState(Behaviour), true);
    }

}
