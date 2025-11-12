public class LadronzueloApproachState : LadronzueloState {

    //Constructor
    public LadronzueloApproachState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check player visibility
        if (!Enemy.PlayerIsVisible) {
            //Player not visible -> Go to idle
            Behaviour.SetState(new LadronzueloIdleState(Behaviour));
        } else if (Enemy.PlayerDistance > Ladronzuelo.InteractRange) {
            //Player too far -> Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
        } else {
            //Player in interact range -> Check if allowed to steal
            if (Ladronzuelo.CheckIfAllowedToSteal()) {
                //Steal from player
                Behaviour.SetState(new LadronzueloStealState(Behaviour), true);
            } else if (Ladronzuelo.StolenAmount > 0) {
                //Stole gold -> Flee from player
                Behaviour.SetState(new LadronzueloFleeState(Behaviour));
            } else {
                //Stole nothin -> Attack player
                Behaviour.SetState(new LadronzueloAttackState(Behaviour), true);
            }
        }
    }

}
