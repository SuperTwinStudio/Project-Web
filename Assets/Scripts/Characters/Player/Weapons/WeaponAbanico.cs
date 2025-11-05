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
    [SerializeField, Min(0)] private float secondaryPushForce = 2f;
    [SerializeField, Min(0)] private float secondaryPushForcePerLevel = 1f;
    [SerializeField, Min(0)] private Vector2 secondaryAttackSphereCast = new(1.5f, 0f);

    private float SecondaryPushForce => secondaryPushForce + (SecondaryUpgrade.Level - 1) * secondaryPushForcePerLevel;

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passivePushForce = 2f;
    [SerializeField, Min(0)] private float passivePushForcePerLevel = 1f;
    [SerializeField, Min(2)] private int passiveHit = 5;

    private bool isPassiveHit = false;
    private int hitCount = 0;

    private float PassivePushForce => passivePushForce + (PassiveUpgrade.Level - 1) * passivePushForcePerLevel;

    //Ammo
    [Header("Ammo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;


    //State
    protected override void OnShow() {
        //Reset passive
        hitCount = 0;
        UpdatePassiveValue();
    }

    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponAction.Secondary, primarySecondaryCooldown);

        //Shoot
        Projectile projectile = SpawnProjectile(bulletPrefab, bulletOrigin).GetComponent<Projectile>();
        projectile.Init(Player, PrimaryDamage);
        if (isPassiveHit) projectile.AddOnHit((damageable) => Push(damageable, PassivePushForce));

        //Next hit
        hitCount = (hitCount + 1) % passiveHit;
        isPassiveHit = hitCount == passiveHit - 1;
        UpdatePassiveValue();

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
            (damageable) => Push(damageable, SecondaryPushForce)
        );
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(WeaponAction.Passive, passiveHit - hitCount - 1);
    }

    //Helpers    
    private void Push(IDamageable damageable, float force) {
        //Check if a character
        if (damageable is not Character) return;

        //Push enemy back
        Character character = damageable as Character;
        Debug.Log($"Push {character.name} with {force} force");
    }

}
