public class ThiefIdleState : ThiefState {

    //Constructor
    public ThiefIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Update gold text
        Thief.UpdateGoldText();
    }

    public override void Execute() {
        //Check if target is visible
        if (!Enemy.TargetIsVisible) return;

        //Aproach target
        Behaviour.SetState(new ThiefApproachState(Behaviour), true);
    }

}
