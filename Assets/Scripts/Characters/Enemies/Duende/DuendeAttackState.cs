public class DuendeAttackState : DuendeState {

    //Constructor
    public DuendeAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Stop moving
        Enemy.StopMovement();

        //Attack
        Duende.Attack();
    }

    //Attack
    public override void Execute() {
        //Cannot attack
        if (Duende.OnAttackCooldown) return;

        //Check distance
        if (Enemy.TargetLastKnownDistance < Duende.EvadeRange) {
            //Target too close -> Evade
            Duende.SetState(new DuendeEvadeState(Duende));
        } else if (Enemy.TargetLastKnownDistance < Duende.MaxAttackRange) {
            //Target in range -> Attack
            Duende.Attack();
        } else {
            //Target fuera de rango -> Siguele
            Duende.SetState(new DuendeFollowState(Duende));
        }
    }

}
