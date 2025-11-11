using UnityEngine;

public class LadronzueloApproachState : LadronzueloState {

    //Constructor
    public LadronzueloApproachState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnExit() {
        //Stop movement
        Enemy.StopMovement();
        Enemy.Animator.SetBool("IsMoving", false);
    }

    public override void Execute() {
        //Check player visibility
        if (!Enemy.PlayerIsVisible) {
            //Player not visible -> Go to idle
            Behaviour.SetState(new LadronzueloIdleState(Behaviour));
        } else if (Enemy.PlayerDistance <= Ladronzuelo.AttackRange) {
            if (OtherTypes()) {
                //Player in attack range -> Steal from it
                Behaviour.SetState(new LadronzueloStealState(Behaviour), true);
            } else {
                //Player in attack range -> Attack it
                Behaviour.SetState(new LadronzueloAttackState(Behaviour), true);
            }
        } else {
            //Move towards player
            Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
            Enemy.Animator.SetBool("IsMoving", true);
        }
    }

    private bool OtherTypes() {
        foreach (EnemyBase enemy in Enemy.Room.Enemies) {
            //Ignore ladronzuelos
            if (enemy.Behaviour is LadronzueloBehaviour) continue;

            //Found a different type
            return true;
        }

        //No other types
        return false;
    }

}
