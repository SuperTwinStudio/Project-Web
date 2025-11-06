using UnityEngine;

public class EnemyTest : EnemyBase {

    public override bool Damage(float amount, object source, DamageType type = DamageType.None) {
        Debug.Log($"{name}: -{amount * damageTakenMultiplier} HP");
        return base.Damage(amount, source, type);
    }

    protected override void OnDeath() {
        base.OnDeath();

        //Destroy
        Destroy(gameObject);
    }

}
