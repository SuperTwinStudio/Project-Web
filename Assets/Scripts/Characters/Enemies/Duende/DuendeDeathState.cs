public class DuendeDeathState : DuendeState {

    //Constructor
    public DuendeDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Animate
        Enemy.Animator.SetTrigger("Die");

        //Play sound
        //Enemy.PlaySound(Duende.DeathSound);
    }

}