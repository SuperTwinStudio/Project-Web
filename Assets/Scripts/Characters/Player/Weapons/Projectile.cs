using UnityEngine;

public class Projectile : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;

    //Projectile
    [Header("Projectile")]
    public bool isPlayer = true;
    public float speed = 10;
    public float damage = 10;


    //State
    private void Start() {
        //Apply velocity
        rigidbody.linearVelocity = speed * transform.forward;
    }

    private void OnTriggerEnter(Collider other) {
        //Ignore player/other
        if (other.CompareTag("Player") == isPlayer) return;

        //Check if damageable
        if (!other.TryGetComponent(out IDamageable damageable)) return;

        //Deal damage & destroy self
        damageable.Damage(damage);
        Destroy(gameObject);
    }

}
