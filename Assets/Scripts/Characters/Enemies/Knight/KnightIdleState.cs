public class KnightIdleState : KnightState {

    //Constructor
    public KnightIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Hide shield
        Knight.ToggleShield(false);
    }

    public override void Execute() {
        //Check if target is visible
        if (!Enemy.TargetIsVisible) return;

        //Follow target
        Behaviour.SetState(new KnightFollowState(Behaviour), true);
    }

}
