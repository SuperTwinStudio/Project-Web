using System.Collections;
using UnityEngine;

public class DuendeAttackState : DuendeState {

    //Constructor
    public DuendeAttackState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Stop moving
        Enemy.StopMovement();

        //Attack
        Duende.Attack();
    }

    //Attack
    public override void Execute() {
        //Cannot attack
        if (Duende.OnAttackCooldown) return;

        //Check distance
        if (Enemy.PlayerDistance < Duende.EvadeRange) {
            //Player too close - > evade
            Duende.SetState(new DuendeEvadeState(Duende));
        } else if (Enemy.PlayerDistance < Duende.MaxAttackRange) {
            //Attack
            Duende.Attack();
        } else {
            //Player fuera de rango -> siguele
            Duende.SetState(new DuendeFollowState(Duende));
        }
    }

}
