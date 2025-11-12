public class DuendeFollowState : DuendeState {

    //Constructor
    public DuendeFollowState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check player visibility
        if (!Enemy.PlayerIsVisible) {
            //Player not visible -> Go to idle
            Duende.SetState(new DuendeIdleState(Duende));
        } else if (Enemy.PlayerDistance > Duende.EvadeRange && Enemy.PlayerDistance < Duende.MinAttackRange) {
            //Player in attack range -> Attack it
            Enemy.StopMovement();
            Duende.SetState(new DuendeAttackState(Duende));
        } else {
            //Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
        }
    }

}
