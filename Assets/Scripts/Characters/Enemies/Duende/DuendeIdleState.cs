public class DuendeIdleState : DuendeState {

    //Constructor
    public DuendeIdleState(EnemyBehaviour Duende) : base(Duende) {}

    //Actions
    public override void Execute() {
        //Check if player is visible
        if (!Enemy.PlayerIsVisible) return;

        //Check distance
        if (Enemy.PlayerDistance > Duende.MinAttackRange) {
            //Player too far -> follow him
            Duende.SetState(new DuendeFollowState(Duende));
        } else if (Enemy.PlayerDistance > Duende.EvadeRange) {
            //Player within attack range -> Attack him
            Duende.SetState(new DuendeAttackState(Duende));
        } else {
            //Player too close -> Evade
            Duende.SetState(new DuendeEvadeState(Duende));
        }
    }

}
