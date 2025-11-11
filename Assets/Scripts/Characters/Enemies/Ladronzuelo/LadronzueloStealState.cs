using Botpa;

public class LadronzueloStealState : LadronzueloState {

    //Attack
    private int stolenAmount = 0;


    //Constructor
    public LadronzueloStealState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
        Enemy.Animator.SetBool("IsMoving", false);
    }

    public override void Execute() {
        if (Enemy.PlayerDistance <= Ladronzuelo.StealRange) {
            //Player in steal range -> Steal from it
            // If the inventary is empty, the enemy attacks the player
            if (Enemy.Player.Loadout.Inventory.IsEmpty()) {
                Behaviour.SetState(new LadronzueloAttackState(Behaviour), true);
            } else {
                Enemy.Player.Loadout.SpendGold(Ladronzuelo.StealAmount);
                stolenAmount += Ladronzuelo.StealAmount; //Esta variable sirve para luego poder devolverle al personaje el oro robado
            }
        } else {
            // If the player is far, the enemy approaches the player
            Behaviour.SetState(new LadronzueloApproachState(Behaviour), true);
        }
    }

}
