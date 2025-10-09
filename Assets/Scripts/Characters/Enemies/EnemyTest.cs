using UnityEngine;

public class EnemyTest : EnemyBase {

    public override bool Damage(float amount) {
        Debug.Log($"{name}: -{amount} HP");
        return base.Damage(amount);
    }

    protected override void OnDeath(bool instant = false) {
        base.OnDeath(instant);

        //Destroy
        Destroy(gameObject);
    }

}
