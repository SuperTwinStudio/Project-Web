using UnityEngine;

public class EnemyTest : EnemyBase {

    public override bool Damage(float amount) {
        Debug.Log($"{name}: -{amount} HP");
        return base.Damage(amount);
    }

}
