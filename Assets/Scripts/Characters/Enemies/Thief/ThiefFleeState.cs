using System.Linq;
using UnityEngine;

public class ThiefFleeState : ThiefState {

    //Constructor
    public ThiefFleeState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnEnter() {
        //Add flee effect (extra move speed)
        Enemy.AddEffect(Thief.FleeEffect);

        //Flee from player
        Flee();
    }

    public override void OnExit() {
        //Remove flee effect
        Enemy.RemoveEffect(Thief.FleeEffect);

        //Stop movement & enable rotation
        Enemy.StopMovement();
        Enemy.SetAutomaticRotation(true);
    }

    public override void Execute() {
        //Stopped
        if (Enemy.Agent.isStopped) return;

        //Check if reached destination
        if (Enemy.AgentReachedDestination) {
            //Stop movement & enable rotation
            Enemy.StopMovement();
            Enemy.SetAutomaticRotation(true);
        }
    }

    public override void OnDamage(DamageType type, object source) {
        //Already fleeing
        if (!Enemy.Agent.isStopped) return;

        //Flee from player
        if (type != DamageType.Burn) Flee();
    }

    //Helpers
    private void Flee() {
        //Look for a good flee spot & move towards it
        Vector3 randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
        (Vector3 point1, float distance1) = Enemy.GetFurthestPointAndDistance(randomDirection, Thief.MaxFleeDistance);
        (Vector3 point2, float distance2) = Enemy.GetFurthestPointAndDistance(Quaternion.AngleAxis(90, Vector3.up) * randomDirection, Thief.MaxFleeDistance);
        (Vector3 point3, float distance3) = Enemy.GetFurthestPointAndDistance(Quaternion.AngleAxis(180, Vector3.up) * randomDirection, Thief.MaxFleeDistance);
        (Vector3 point4, float distance4) = Enemy.GetFurthestPointAndDistance(Quaternion.AngleAxis(270, Vector3.up) * randomDirection, Thief.MaxFleeDistance);

        //Get closest pair
        var closestPair = new PointDistancePair[] {
            new() { point = point1, distance = distance1 },
            new() { point = point2, distance = distance2 },
            new() { point = point3, distance = distance3 },
            new() { point = point4, distance = distance4 }
        }.OrderBy(data => data.distance).LastOrDefault();

        //Move to point
        Enemy.MoveTowards(closestPair.point);
        Enemy.SetAutomaticRotation(false);
        Enemy.LookTowards(closestPair.point);
    
        //Play sound
        Enemy.PlaySound(Thief.FleeSound);
    }

    private struct PointDistancePair {

        public Vector3 point;
        public float distance;

    }

}
