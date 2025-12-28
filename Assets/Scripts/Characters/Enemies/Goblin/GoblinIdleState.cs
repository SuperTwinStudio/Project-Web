public class GoblinIdleState : GoblinState {

    //Constructor
    public GoblinIdleState(EnemyBehaviour Duende) : base(Duende) {}

    //Actions
    public override void Execute() {
        //Check if target position is known
        if (!Enemy.TargetPositionIsKnown) return;

        //Check distance
        if (Enemy.TargetLastKnownDistance > Goblin.MinAttackRange) {
            //Target too far -> Follow it
            Behaviour.SetState(new GoblinFollowState(Behaviour));
        } else if (Enemy.TargetLastKnownDistance > Goblin.EvadeRange) {
            //Target within attack range -> Attack it
            Behaviour.SetState(new GoblinAttackState(Behaviour));
        } else {
            //Target too close -> Evade it
            Behaviour.SetState(new GoblinEvadeState(Behaviour));
        }
    }

}
