public class DuendeIdleState : DuendeState {

    //Constructor
    public DuendeIdleState(EnemyBehaviour Duende) : base(Duende) {}

    //Actions
    public override void Execute() {
        //Check if target is visible
        if (!Enemy.TargetIsVisible) return;

        //Check distance
        if (Enemy.TargetLastKnownDistance > Duende.MinAttackRange) {
            //Target too far -> Follow it
            Duende.SetState(new DuendeFollowState(Duende));
        } else if (Enemy.TargetLastKnownDistance > Duende.EvadeRange) {
            //Target within attack range -> Attack it
            Duende.SetState(new DuendeAttackState(Duende));
        } else {
            //Target too close -> Evade it
            Duende.SetState(new DuendeEvadeState(Duende));
        }
    }

}
