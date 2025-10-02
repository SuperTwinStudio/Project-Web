using Botpa;
using UnityEngine;

public enum PlayerClassCategory { Melee, Ranged, Effect }

public class PlayerClass : MonoBehaviour {

    //Item
    [Header("Item")]
    [SerializeField] private Item _item;

    public Item Item => _item;

    //Primary
    [Header("Primary")]
    [SerializeField] private PlayerClassCategory primaryCategory = PlayerClassCategory.Melee;
    [SerializeField, Min(0)] private float primaryCooldown = 1;
    [SerializeField, Min(0)] private float primaryDamage = 1;
    [SerializeField] private Cinematic primaryOnUse;

    public bool PrimaryCanUse => !primaryOnUse.isPlaying;

    //Primary
    /*
    [Header("Primary")]
    [SerializeField] private PlayerClassCategory secondaryCategory = PlayerClassCategory.Effect;
    [SerializeField] private float secodaryRange = 1;
    */

    //Use
    public void UsePrimary() {
        primaryOnUse.Play();
    }

    public void UseSecondary() {
        //
    }

    //Actions
    public void AtackMelee(float range) {
        float radius = range / 2;
        var collisions = Physics.SphereCastAll(transform.position + radius * transform.forward, radius, Vector3.zero, 0);
        foreach (var collision in collisions) {
            Debug.Log(collision.collider);

            //Check if collision is a damageable
            if (!collision.collider.TryGetComponent(out IDamageable damageable)) continue;

            //Damage
            damageable.Damage(primaryDamage);
        }
    }

    public void SpawnProjectile(GameObject prefab) {

    }

}
