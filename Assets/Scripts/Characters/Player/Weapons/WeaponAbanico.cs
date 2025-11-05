using System.Collections;
using UnityEngine;

public class WeaponAbanico : Weapon {

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.2f;
    [SerializeField, Min(0)] private float primaryDamage = 10f;
    [SerializeField, Min(0)] private float primaryDamagePerLevel = 5f;

    private float PrimaryDamage => primaryDamage + (PrimaryUpgrade.Level - 1) * primaryDamagePerLevel;

    protected override float PrimaryCooldownDuration => _primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 5f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.3f;
    [SerializeField, Min(0)] private Vector2 secondaryAttackSphereCast = new(1.5f, 0f);

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Ammo
    [Header("Ammo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;


    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponAction.Secondary, primarySecondaryCooldown);

        //Shoot
        SpawnProjectile(bulletPrefab, bulletOrigin).GetComponent<Projectile>().Init(Player, PrimaryDamage);

        //Animate
        animator.SetTrigger("Shoot");

        //Apply camera knockback
        CameraController.AddKnockback(-transform.forward);
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while using secondary
        SetCooldown(WeaponAction.Primary, secondaryPrimaryCooldown);

        //Push enemies back
        MeleeForward(
            secondaryAttackSphereCast.x,
            secondaryAttackSphereCast.y,
            0,
            (damageable) => {
                //Check if a character
                if (damageable is not Character) return;

                //Push enemy back
                Character character = damageable as Character;
                Debug.Log($"Push {character.name}");
            }
        );
    }

}
