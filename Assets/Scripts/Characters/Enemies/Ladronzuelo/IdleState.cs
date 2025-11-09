
using UnityEngine;

public class IdleState : EnemyState
{
    GameObject[] enemies;

    //Constructor
    public IdleState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnEnter()
    {
        //Called when the state enters
    }

    public override void OnExit()
    {
        //Called when the state exits
    }

    public override void Execute()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Check if player is visible
        if (!Enemy.PlayerIsVisible) return;

        // See if the player is near and decide which action make based on if there are other types of enemies
        if(!PlayerNear())
        {
            Behaviour.SetState(new ApproachState(Behaviour), true);
        } else if(OtherTypes())
        {
            Behaviour.SetState(new StealState(Behaviour), true);

        } else
        {
            Behaviour.SetState(new AttackState(Behaviour), true);
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

    private bool PlayerNear()
    {
        float threshold = 1.5f;
        return Enemy.PlayerDistance <= threshold;
    }

}
