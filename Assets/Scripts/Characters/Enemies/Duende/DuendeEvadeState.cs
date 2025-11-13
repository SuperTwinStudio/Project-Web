public class DuendeEvadeState : DuendeState {

    //Constructor
    public DuendeEvadeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        if (Enemy.TargetLastKnownDistance > Duende.EvadeRange) {
            //Target lejos -> Mira a ver que haces
            Duende.SetState(new DuendeIdleState(Duende));
        } else {
            //Target cerca -> Huye!
            Enemy.MoveTowards(Enemy.Bot.position + (Enemy.Eyes.position - Enemy.TargetLastKnownPosition).normalized);
        }
    }

}
