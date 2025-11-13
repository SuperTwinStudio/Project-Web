public class KnightFollowState : KnightState {

    //Constructor
    public KnightFollowState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Check player visibility
        if (!Enemy.TargetIsVisible) {
            //Player not visible -> Go to idle
            Behaviour.SetState(new KnightIdleState(Behaviour));
        } else if (Enemy.TargetLastKnownDistance <= Knight.AttackRange) {
            //Player in attack range -> Attack it
            Behaviour.SetState(new KnightSheatheState(Behaviour), true);
        } else {
            //Move towards player
            Enemy.MoveTowards(Enemy.TargetLastKnownPosition);
        }
    }

}
