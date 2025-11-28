public class GoblinIdleState : GoblinState {

    //Constructor
    public GoblinIdleState(EnemyBehaviour Duende) : base(Duende) {}

    //Actions
    public override void Execute() {
        //Check if target is visible
        if (!Enemy.TargetIsVisible) return;

        //Check distance
        if (Enemy.TargetLastKnownDistance > Goblin.MinAttackRange) {
            //Target too far -> Follow it
            Goblin.SetState(new GoblinFollowState(Goblin));
        } else if (Enemy.TargetLastKnownDistance > Goblin.EvadeRange) {
            //Target within attack range -> Attack it
            Goblin.SetState(new GoblinAttackState(Goblin));
        } else {
            //Target too close -> Evade it
            Goblin.SetState(new GoblinEvadeState(Goblin));
        }
    }

}
