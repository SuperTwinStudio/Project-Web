public class ThiefDeathState : ThiefState {

    //Constructor
    public ThiefDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Return gold
        Thief.ReturnGoldToPlayer();

        //Animate
        Enemy.Animator.SetTrigger("Die");

        //Play sound
        Enemy.PlaySound(Thief.DeathSound);
    }

}
