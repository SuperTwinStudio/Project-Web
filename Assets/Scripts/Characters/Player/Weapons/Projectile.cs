using UnityEngine;

public class Projectile : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;
    private Loadout _loadout;

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

    public void SetLoadout(Loadout loadout)
    {
        _loadout = loadout;
    }

    private void OnTriggerEnter(Collider other) {
        //Ignore player/other
        if (other.CompareTag("Player") == isPlayer) return;

        //Check if damageable
        if (!other.TryGetComponent(out IDamageable damageable)) return;

        //Deal damage & destroy self
        _loadout.OnDamageableHit(other.gameObject);
        damageable.Damage(damage, _loadout.Player);
        Destroy(gameObject);
    }

}
