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
            Goblin.SetState(new GoblinFollowState(Goblin));
        } else if (Enemy.TargetLastKnownDistance < Goblin.EvadeRange) {
            //Target too close -> Evade
            Goblin.SetState(new GoblinEvadeState(Goblin));
        } else if (Enemy.TargetLastKnownDistance < Goblin.MaxAttackRange) {
            //Target in range -> Attack
            Goblin.Attack();
        } else {
            //Target fuera de rango -> Siguele
            Goblin.SetState(new GoblinFollowState(Goblin));
        }
    }

}
