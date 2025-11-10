using System;
using UnityEngine;

public class Projectile : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;

    private object source = null;

    //Projectile
    [Header("Projectile")]
    [SerializeField] private bool isPlayer = true;
    [SerializeField] private bool destroyOnDamage = true;
    [SerializeField] private bool destroyOnWallHit = true;
    [SerializeField] private float speed = 20;
    [SerializeField] private float damage = 25;

    private event Action<IDamageable> OnHit;


    //State
    private void Start() {
        //Apply velocity
        rigidbody.linearVelocity = speed * transform.forward;
    }

    public void Init(object source, float damage = -1) {
        //Save origin
        this.source = source;

        //Update damage (optional value)
        if (damage > 0) this.damage = damage;
    }

    private void OnTriggerEnter(Collider other) {
        //Collided with trigger
        if (other.isTrigger) return;

        //Ignore certain targets (player hits everything except player, enemies only hit player)
        if (isPlayer == other.CompareTag("Player")) return;

        //Check if damageable
        if (other.TryGetComponent(out IDamageable damageable)) {
            //Deal damage
            damageable.Damage(damage, source, DamageType.Ranged);
            if (isPlayer) Game.Current.Level.Player.Loadout.OnDamageableHit(other.gameObject);

            //Call on hit event
            OnHit?.Invoke(damageable);

            //Check if should destroy self
            if (destroyOnDamage) Destroy(gameObject);
        } else {
            //Check if should destroy self
            if (destroyOnWallHit) Destroy(gameObject);
        }
    }

    //Events
    public void AddOnHit(Action<IDamageable> action) {
        OnHit += action;
    }

    public void RemoveOnHit(Action<IDamageable> action) {
        OnHit -= action;
    }

}
