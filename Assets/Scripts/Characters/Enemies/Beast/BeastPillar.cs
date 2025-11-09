using UnityEngine;

public class BeastPillar : MonoBehaviour, IDamageable {

    public BeastBehaviour Beast { get; private set; }

    //Pillar
    [Header("Pillar")]
    [SerializeField] private Transform _top;

    public Transform Top => _top;

    //Health
    [Header("Health")]
    [SerializeField] private float _maxHealth = 300;
    [SerializeField] private GameObject damageIndicatorPrefab;

    public bool IsAlive { get; private set; } = true;
    public float Health { get; private set; } = 0;
    public float HealthMax => _maxHealth;


    //State
    public void Init(BeastBehaviour beast) {
        //Save beast
        Beast = beast;

        //Refill health
        Heal(HealthMax);
    }

    //Health
    public bool Damage(float amount, object source, DamageType type = DamageType.None) {
        if (!IsAlive) return false;

        Health = Mathf.Clamp(Health - amount, 0, Health);

        Instantiate(damageIndicatorPrefab, Top.position + 0.3f * Vector3.up, Quaternion.identity).GetComponent<DamageTextIndicator>().SetDamage(amount, type);

        if (Health <= 0) {
            IsAlive = false;

            Beast.OnPillarDestroyed(this);

            Destroy(gameObject);
        }

        return true;
    }

    public bool Heal(float amount) {
        if (!IsAlive) return false;

        Health = Mathf.Clamp(Health + amount, Health, HealthMax);

        return true;
    }

    public bool Revive(float health) { return true; }

}
