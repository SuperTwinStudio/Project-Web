public class KnightAttackState : KnightState {

    //Constructor
    public KnightAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Disable autorotation
        Enemy.SetAutomaticRotation(false);

        //Hide shield
        Knight.ToggleShield(false);

        //Animate attack
        Enemy.Animator.SetTrigger("Attack");
    }

    public override void OnExit() {
        //Reenable autorotation
        Enemy.SetAutomaticRotation(true);
    }

}
