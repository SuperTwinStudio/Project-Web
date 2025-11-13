public class LadronzueloIdleState : LadronzueloState {

    //Constructor
    public LadronzueloIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Update gold text
        Ladronzuelo.UpdateGoldText();
    }

    public override void Execute() {
        //Check if target is visible
        if (!Enemy.TargetIsVisible) return;

        //Aproach target
        Behaviour.SetState(new LadronzueloApproachState(Behaviour), true);
    }

}
