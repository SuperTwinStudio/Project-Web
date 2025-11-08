using System.Collections;
using Botpa;
using UnityEngine;

public class WeaponAbanico : Weapon {

    //Temp
    [Header("Temp")]
    [SerializeField] private Animator _animator;

    protected override Animator Animator => _animator;

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.2f;
    [SerializeField, Min(0)] private float primaryDamage = 10f;
    [SerializeField, Min(0)] private float primaryDamagePerLevel = 5f;
    [SerializeField] private AudioClip primaryAttackSound;

    public override float PrimaryCooldownDuration => _primaryCooldown;

    public override float PrimaryDamage => primaryDamage + (PrimaryUpgrade.Level - 1) * primaryDamagePerLevel;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float secondaryPushForce = 10f;
    [SerializeField, Min(0)] private float secondaryPushForcePerLevel = 3f;
    [SerializeField] private Vector2 secondaryAttackSphereCast = new(1.5f, 0f);
    [SerializeField] private AudioClip secondaryAttackSound;

    private float SecondaryPushForce => secondaryPushForce + (SecondaryUpgrade.Level - 1) * secondaryPushForcePerLevel;

    public override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passivePushForce = 6f;
    [SerializeField, Min(0)] private float passivePushForcePerLevel = 2f;
    [SerializeField, Min(1)] private int passiveHit = 8;
    [SerializeField] private AudioClip passiveAttackSound;

    private bool isPassiveHit = false;
    private int hitCount = 0;

    private float PassivePushForce => passivePushForce + (PassiveUpgrade.Level - 1) * passivePushForcePerLevel;

    public override float PassiveCooldown => isPassiveHit ? 0 : 1;

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

    //Weapon 
    private void Push(IDamageable damageable, float force, Vector3 direction = new Vector3()) {
        //Check if an enemy
        if (damageable is not EnemyBase) return;

        //Get enemy
        EnemyBase enemy = damageable as EnemyBase;

        //Push enemy
        if (direction.IsEmpty()) direction = enemy.transform.position - Player.transform.position;
        enemy.Push(force * direction);
    }

    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponAction.Secondary, primarySecondaryCooldown);

        //Shoot
        Projectile projectile = SpawnProjectile(bulletPrefab, PrimaryDamage, bulletOrigin);
        if (isPassiveHit) projectile.AddOnHit((damageable) => Push(damageable, PassivePushForce));

        //Animate
        PlaySound(isPassiveHit ? passiveAttackSound : primaryAttackSound);
        Animator.SetTrigger("Shoot");

        //Next hit
        hitCount = (hitCount + 1) % passiveHit;
        isPassiveHit = hitCount == passiveHit - 1;
        UpdatePassiveValue();

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

        //Animate
        PlaySound(secondaryAttackSound);
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(WeaponAction.Passive, passiveHit - hitCount - 1);
    }

}
