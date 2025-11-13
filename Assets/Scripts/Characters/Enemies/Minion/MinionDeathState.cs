public class MinionDeathState : MinionState {

    //Constructor
    public MinionDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Animate
        Enemy.Animator.SetTrigger("Die");
    }

}
