public class KnightIdleState : KnightState {

    //Constructor
    public KnightIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Hide shield
        Knight.ToggleShield(false);
    }

    public override void Execute() {
        //Check if target position is known
        if (!Enemy.TargetPositionIsKnown) return;

        //Follow target
        Behaviour.SetState(new KnightFollowState(Behaviour));
    }

}
