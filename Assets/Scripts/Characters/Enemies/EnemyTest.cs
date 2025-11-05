using UnityEngine;

public class EnemyTest : EnemyBase {

    public override bool Damage(float amount, object source, DamageType type = DamageType.None) {
        Debug.Log($"{name}: -{amount} HP");
        return base.Damage(amount, source, type);
    }

    protected override void OnDeath(bool instant = false) {
        base.OnDeath(instant);

        //Destroy
        Destroy(gameObject);
    }

}
