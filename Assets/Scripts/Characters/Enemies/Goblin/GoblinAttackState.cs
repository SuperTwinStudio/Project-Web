public class GoblinAttackState : GoblinState {

    //Constructor
    public GoblinAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Stop moving
        Enemy.StopMovement();

        //Attack
        Goblin.Attack();
    }

    //Attack
    public override void Execute() {
        //Cannot attack
        if (Goblin.OnAttackCooldown) return;

        //Check distance
        if (!Enemy.TargetIsVisible) {
            //Lost target -> Go to last known position
            Behaviour.SetState(new GoblinFollowState(Behaviour));
        } else if (Enemy.TargetLastKnownDistance < Goblin.EvadeRange) {
            //Target too close -> Evade
            Behaviour.SetState(new GoblinEvadeState(Behaviour));
        } else if (Enemy.TargetLastKnownDistance < Goblin.MaxAttackRange) {
            //Target in range -> Attack
            Goblin.Attack();
        } else {
            //Target fuera de rango -> Siguele
            Behaviour.SetState(new GoblinFollowState(Behaviour));
        }
    }

}
