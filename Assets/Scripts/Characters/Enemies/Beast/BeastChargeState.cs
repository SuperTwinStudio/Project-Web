using Botpa;
using UnityEngine;

public class BeastChargeState : BeastState {

    //Charge
    private readonly Timer timeoutTimer = new(); //To prevent the enemy from staying permanently in charge mode (happened once in testing)

    private const float TIMEOUT_DURATION = 2.0f;
    private const float AOE_RADIUS = 2.0f;


    //Constructor
    public BeastChargeState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Play sound
        Enemy.PlaySound(Beast.ChargeSound);

        //Get furthest point forward
        (Vector3 point, float distance) = Enemy.GetFurthestPointAndDistance(Enemy.Model.forward, Beast.MaxChargeDistance);

        //Move to point
        Enemy.MoveTowards(point);
        timeoutTimer.Count(TIMEOUT_DURATION);

        //Show AOE indicator
        Enemy.Attack.ShowAOE(AOE_RADIUS, Mathf.Max(0, distance - AOE_RADIUS), true, 0.5f);
    }

    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
    }

    public override void Execute() {
        //Aura check
        base.Execute();

        //Check if reached destination
        if (Enemy.AgentReachedDestination || timeoutTimer.IsFinished) {
            //Reached destination -> Go to stunned state
            Behaviour.SetState(new BeastStunState(Behaviour));
        }
    }

}
