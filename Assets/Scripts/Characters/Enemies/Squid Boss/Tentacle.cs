using UnityEngine;

public class Tentacle : MonoBehaviour, IDamageable
{
    [SerializeField] private SquidBossBehaviour m_Squid;

    public bool IsAlive { get; set; } = true;

    public float Health { get; set; } = 200;

    public float HealthMax { get; set; } = 200;

    public bool Damage(float amount, object source, DamageType type = DamageType.None)
    {
        Health -= amount;

        if (Health > 0) 
        { 
            IsAlive = false;
            m_Squid.RemoveTentacle(gameObject);
            Destroy(gameObject);
        }

        return true;
    }

    public bool Heal(float amount)
    {
        // CANNOT HEAL
        return false;
    }

    public bool Revive(float health)
    {
        // CANNOT REVIVE
        return false;
    }
}
