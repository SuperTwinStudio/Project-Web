using Botpa;
using UnityEngine;

public class BeastState : EnemyState {

    //Beast
    private readonly Timer damageTimer = new();

    protected BeastBehaviour Beast => Behaviour as BeastBehaviour;


    //Constructor
    public BeastState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void Execute() {
        //Enemy is vulnerable, aura is in cooldown or not triggering
        if (!Enemy.IsInvulnerable || damageTimer.counting || !Beast.AuraDetector.isTriggered) return;

        //Calculate direction towards player
        Vector3 direction = (Enemy.Player.transform.position - Enemy.Model.position).normalized;

        //Damage & player
        Enemy.Player.Damage(Beast.AuraDamage, Enemy, DamageType.Melee);
        Enemy.Player.Push(Beast.AuraPushForce * direction);

        //Start cooldown
        damageTimer.Count(Beast.AuraCooldown);
    }

}
