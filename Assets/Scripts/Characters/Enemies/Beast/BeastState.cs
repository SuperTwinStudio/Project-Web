using Botpa;
using UnityEngine;

public class BeastState : EnemyState {

    //Beast
    private readonly Timer damageTimer = new();

    protected BeastBehaviour Beast { get; private set; }


    //Constructor
    public BeastState(EnemyBehaviour behaviour) : base(behaviour) {
        Beast = behaviour as BeastBehaviour;
    }

    //Actions
    public override void Execute() {
        //Enemy is vulnerable, aura is in cooldown or not triggering
        if (!Enemy.IsInvulnerable || damageTimer.IsCounting || !Beast.AuraDetector.isTriggered) return;

        //Calculate direction towards player
        Vector3 direction = (Enemy.Player.transform.position - Enemy.Model.position).normalized;

        //Damage & player
        Enemy.Player.Damage(Beast.AuraDamage, DamageType.Melee, Enemy);
        Enemy.Player.Push(Beast.AuraPushForce * direction);

        //Start cooldown
        damageTimer.Count(Beast.AuraCooldown);
    }

    public override void OnDamage(DamageType type, object source) {
        //Play sound
        if (type != DamageType.Burn) Enemy.PlaySound(Beast.DamageSound);
    }

}
