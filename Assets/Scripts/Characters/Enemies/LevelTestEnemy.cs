using UnityEngine;

public class LevelTestEnemy : EnemyBase
{
    protected override void OnDeath(bool instant = false)
    {
        base.OnDeath(instant);

        //Destroy
        Destroy(gameObject);
    }
}