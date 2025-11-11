public class LadronzueloApproachState : LadronzueloState {

    //Constructor
    public LadronzueloApproachState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
        Enemy.Animator.SetBool("IsMoving", false);
    }

    public override void Execute() {
        //Check player visibility
        if (!Enemy.PlayerIsVisible) {
            //Player not visible -> Go to idle
            Behaviour.SetState(new LadronzueloIdleState(Behaviour));
        } else if (Enemy.PlayerDistance > Ladronzuelo.InteractRange) {
            //Player too far -> Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
            Enemy.Animator.SetBool("IsMoving", true);
        } else {
            //Player in interact range -> Check if allowed to steal
            if (Ladronzuelo.CheckIfAllowedToSteal()) {
                //Steal from player
                Behaviour.SetState(new LadronzueloStealState(Behaviour), true);
            } else {
                //Attack player
                Behaviour.SetState(new LadronzueloAttackState(Behaviour), true);
            }
        }
    }

}
