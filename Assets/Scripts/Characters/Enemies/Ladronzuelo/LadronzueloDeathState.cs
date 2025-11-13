public class LadronzueloDeathState : LadronzueloState {

    //Constructor
    public LadronzueloDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Return gold
        Ladronzuelo.ReturnGoldToPlayer();

        //Animate
        Enemy.Animator.SetTrigger("Die");
    }

}
