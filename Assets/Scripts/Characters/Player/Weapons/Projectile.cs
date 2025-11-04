using UnityEngine;

public class Projectile : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;

    private object source = null;

    //Projectile
    [Header("Projectile")]
    [SerializeField] private bool isPlayer = true;
    [SerializeField] private float speed = 20;
    [SerializeField] private float damage = 25;


    //State
    private void Start() {
        //Apply velocity
        rigidbody.linearVelocity = speed * transform.forward;
    }

    public void Init(object source, float damage = -1) {
        //Save origin
        this.source = source;

        //Update damage
        if (damage > 0) this.damage = damage;
    }

    private void OnTriggerEnter(Collider other) {
        //Ignore player/other
        if (other.CompareTag("Player") == isPlayer) return;

        //Check if damageable
        if (!other.TryGetComponent(out IDamageable damageable)) return;

        //Deal damage & destroy self
        if (isPlayer) Game.Current.Level.Player.Loadout.OnDamageableHit(other.gameObject);
        damageable.Damage(damage, source, DamageType.Ranged);
        Destroy(gameObject);
    }

}
