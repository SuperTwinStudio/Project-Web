public class GoblinEvadeState : GoblinState {

    //Constructor
    public GoblinEvadeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        if (Enemy.TargetLastKnownDistance > Goblin.EvadeRange) {
            //Target lejos -> Mira a ver que haces
            Goblin.SetState(new GoblinIdleState(Goblin));
        } else {
            //Target cerca -> Huye!
            Enemy.MoveTowards(Enemy.Bot.position + (Enemy.Eyes.position - Enemy.TargetLastKnownPosition).normalized);
        }
    }

}
