public class ThiefIdleState : ThiefState {

    //Constructor
    public ThiefIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void Execute() {
        //Check if target position is known
        if (!Enemy.TargetPositionIsKnown) return;

        //Follow target
        Behaviour.SetState(new ThiefFollowState(Behaviour));
    }

}
