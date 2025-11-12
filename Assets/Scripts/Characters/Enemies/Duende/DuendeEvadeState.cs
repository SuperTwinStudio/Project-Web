public class DuendeEvadeState : DuendeState {

    //Constructor
    public DuendeEvadeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        if (Enemy.PlayerDistance > Duende.EvadeRange) {
            //Player lejos -> mira a ver que haces
            Duende.SetState(new DuendeIdleState(Duende));
        } else {
            //Player cerca -> huye!
            Enemy.MoveTowards(Enemy.Bot.position + (Enemy.Eyes.position - Enemy.PlayerLastKnownPosition).normalized);
        }
    }

}
