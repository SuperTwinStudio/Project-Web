using Botpa;

public class StealState : EnemyState
{

    //Attack
    private const float STEAL_RANGE = 1.5f;
    private const int STEAL_AMOUNT = 10;
    private int stolenAmount = 0;
    //Constructor
    public StealState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnEnter()
    {
        //Called when the state enters
    }

    public override void OnExit()
    {
        //Stop movement
        Enemy.StopMovement();
        Enemy.Animator.SetBool("IsMoving", false);
    }

    public override void Execute()
    {
       
        if (Enemy.PlayerDistance <= STEAL_RANGE)
        {
            //Player in steal range -> Steal from it
            // If the inventary is empty, the enemy attacks the player
            if (Enemy.Player.Loadout.Inventory.IsEmpty()) {
                Behaviour.SetState(new AttackState(Behaviour), true);
            }
            else
            {
                Enemy.Player.Loadout.SpendGold(STEAL_AMOUNT);
                // Esta variable sirve para luego poder devolverle al personaje el oro robado
                stolenAmount += STEAL_AMOUNT;
            }
        }
        else
        {
            // If the player is far, the enemy approaches the player
            Behaviour.SetState(new ApproachState(Behaviour), true);
        }
    }

}
