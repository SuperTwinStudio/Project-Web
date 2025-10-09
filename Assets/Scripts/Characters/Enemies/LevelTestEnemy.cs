using UnityEngine;

public class LevelTestEnemy : EnemyBase
{
    public override bool Damage(float amount)
    {
        room.EnemyKilled();
        Destroy(gameObject);
        return true;
    }
}