using UnityEngine;

public class SquidBossMeleeAttackState: EnemyState
{
	//Constructor
    public SquidBossMeleeAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

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
		SquidBossBehaviour squidBehaviour = (SquidBossBehaviour)Behaviour;

        // Choose a random tentacle and attack with it
        int rand = Random.Range(0, squidBehaviour.Tentacles.Count);

    }
}