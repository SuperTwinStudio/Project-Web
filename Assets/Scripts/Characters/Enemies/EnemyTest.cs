using UnityEngine;

public class EnemyTest : EnemyBase {

    public override bool Damage(float amount) {
        Debug.Log(amount);
        return base.Damage(amount);
    }

}
