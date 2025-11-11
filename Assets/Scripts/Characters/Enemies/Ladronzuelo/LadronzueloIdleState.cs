
using UnityEngine;

public class LadronzueloIdleState : LadronzueloState {
    GameObject[] enemies;

    //Constructor
    public LadronzueloIdleState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void Execute() {
        //Check if player is visible
        if (!Enemy.PlayerIsVisible) return;

        // See if the player is near and decide which action make based on if there are other types of enemies
        if (!PlayerNear()) {
            Behaviour.SetState(new LadronzueloApproachState(Behaviour), true);
        } else if (OtherTypes()) {
            Behaviour.SetState(new LadronzueloStealState(Behaviour), true);

        } else {
            Behaviour.SetState(new LadronzueloAttackState(Behaviour), true);
        }
    }

    private bool OtherTypes() {
        foreach (GameObject enemy in enemies) {
            if (enemy.name != "Ladronzuelo") {
                return true;
            }
        }
        return false;
    }

    private bool PlayerNear() {
        float threshold = 1.5f;
        return Enemy.PlayerDistance <= threshold;
    }

}
