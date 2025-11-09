using UnityEngine;

public class AirGust : MonoBehaviour
{
    private Rigidbody m_Rigidbody => GetComponent<Rigidbody>();

    private float m_Speed = 15;

    private void Start()
    {
        //Apply velocity
        m_Rigidbody.linearVelocity = m_Speed * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Collided with trigger
        if (other.isTrigger) return;
        
        //Ignore player/other
        if (other.CompareTag("Player")) return;
        
        //Check if damageable
        if (other.TryGetComponent(out IDamageable damageable))
        {
            if(other.TryGetComponent(out Character character))
            {
                Vector3 direction = other.transform.position - transform.position;
                character.Push(direction * 2);
            }
        }
        
        //Destroy self
        Destroy(gameObject);
    }
}
