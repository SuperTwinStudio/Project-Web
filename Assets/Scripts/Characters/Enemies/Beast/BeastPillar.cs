using UnityEngine;

public class BeastPillar : MonoBehaviour, IDamageable {

    public BeastBehaviour Beast { get; private set; }

    //Pillar
    [Header("Pillar")]
    [SerializeField] private GameObject _model;
    [SerializeField] private Transform _top;

    public GameObject Model => _model;
    public Transform Top => _top;

    //Enemies
    [Header("Enemies")]
    [SerializeField] private Transform _spawn1;
    [SerializeField] private Transform _spawn2;

    public Transform Spawn1 => _spawn1;
    public Transform Spawn2 => _spawn2;

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
            Beast.OnPillarDestroyed(this);

            //Disable & destroy
            Destroy(Model);
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
