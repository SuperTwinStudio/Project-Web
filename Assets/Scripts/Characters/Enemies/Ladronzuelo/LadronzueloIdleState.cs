public class LadronzueloIdleState : LadronzueloState {

    //Constructor
    public LadronzueloIdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Update gold text
        Ladronzuelo.UpdateGoldText();
    }

    public override void Execute() {
        //Check if player is visible
        if (!Enemy.PlayerIsVisible) return;

        //Go to aproach player state & execute it
        Behaviour.SetState(new LadronzueloApproachState(Behaviour), true);
    }

}
