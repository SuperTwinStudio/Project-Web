public class BeastDeathState : BeastState {

    //Constructor
    public BeastDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Animate
        Enemy.Animator.SetTrigger("Die");
        Enemy.PlaySound(Beast.DeathSound);
    }

}
