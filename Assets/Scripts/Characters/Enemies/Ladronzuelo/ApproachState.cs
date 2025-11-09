using UnityEngine;

public class ApproachState : EnemyState
{

    //Attack
    private const float ATTACK_RANGE = 1.5f;
    GameObject[] enemies;


    //Constructor
    public ApproachState(EnemyBehaviour behaviour) : base(behaviour) { }

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
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Check player visibility
        if (!Enemy.PlayerIsVisible)
        {
            //Player not visible -> Go to idle
            Behaviour.SetState(new IdleState(Behaviour));
        }
        else if (Enemy.PlayerDistance <= ATTACK_RANGE)
        {
            if(OtherTypes())
            {
                //Player in attack range -> Steal from it
                Behaviour.SetState(new StealState(Behaviour), true);
            } else
            {
                //Player in attack range -> Attack it
                Behaviour.SetState(new AttackState(Behaviour), true);
            }
        }
        else
        {
            //Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
            Enemy.Animator.SetBool("IsMoving", true);
        }
    }

    private bool OtherTypes()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.name != "Ladronzuelo")
            {
                return true;
            }
        }
        return false;
    }

}
