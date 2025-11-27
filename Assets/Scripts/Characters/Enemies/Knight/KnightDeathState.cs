public class KnightDeathState : KnightState {

    //Constructor
    public KnightDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Animate
        Enemy.Animator.SetTrigger("Die");

        //Play sound
        //Enemy.PlaySound(Duende.DeathSound);
    }

}