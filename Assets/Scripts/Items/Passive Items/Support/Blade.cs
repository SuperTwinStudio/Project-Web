using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_Lifetime;
    private Rigidbody m_Rigidbody => GetComponent<Rigidbody>();

    private void Start()
    {
        //Apply velocity
        m_Rigidbody.linearVelocity = m_Speed * transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        m_Lifetime -= Time.deltaTime;

        if(m_Lifetime <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something");

        if (other.isTrigger) return;

        //Check if damageable
        if (other.TryGetComponent(out IDamageable damageable)) 
        {
            if (other.CompareTag("Player")) return;

            Debug.Log("ENEMY!");

            //Deal damage
            damageable.Damage(15, DamageType.Ranged, Game.Current.Level.Player);

            Destroy(gameObject);
        } 
        else 
        {
            Destroy(gameObject);
        }
    }
}
