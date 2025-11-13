using UnityEngine;

public class BeastPillar : MonoBehaviour, IDamageable {

    //Beast
    public BeastBehaviour Beast { get; private set; }

    //Pillar
    [Header("Pillar")]
    [SerializeField] private Transform _top;

    public Transform Top => _top;

    //Health
    [Header("Health")]
    [SerializeField] private float _baseHealth = 300;

    public bool IsAlive { get; private set; } = true;
    public float Health { get; private set; } = 0;
    public float HealthMax => _baseHealth;

    //Feedback
    [Header("Feedback")]
    [SerializeField] private GameObject damageIndicatorPrefab;


    //State
    public void Init(BeastBehaviour beast) {
        //Save beast
        Beast = beast;

        //Refill health
        Heal(HealthMax);
    }

    //Health
    public bool Damage(float amount, object source, DamageType type = DamageType.None) {
        //Already destroyed
        if (!IsAlive) return false;

        //Lower health
        Health = Mathf.Clamp(Health - amount, 0, Health);

        //Show damage indicator
        Instantiate(damageIndicatorPrefab, Top.position + 0.3f * Vector3.up, Quaternion.identity).GetComponent<DamageTextIndicator>().SetDamage(amount, type);

        //Check if destroyed
        if (Health <= 0) {
            //Update state
            IsAlive = false;

            //Notify pillar was destroyed
            Beast.NotifyPillarDestroyed(this);

            //Disable & destroy
            Destroy(gameObject);
        }

        //Success
        return true;
    }

    public bool Heal(float amount) {
        //Already destroyed
        if (!IsAlive) return false;

        //Heal
        Health = Mathf.Clamp(Health + amount, Health, HealthMax);

        //Success
        return true;
    }

    public bool Revive(float health) { return false; }

}
