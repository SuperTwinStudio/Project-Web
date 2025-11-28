using System.Collections;
using UnityEngine;

public class WeaponFan : Weapon {

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.2f;
    [SerializeField, Min(0)] private float primaryDamage = 15f;
    [SerializeField, Min(0)] private float primaryDamagePerLevel = 5f;
    [SerializeField] private AudioClip primaryAttackSound;

    public override float PrimaryCooldownDuration => _primaryCooldown;

    public override float PrimaryDamage => primaryDamage + (PrimaryUpgrade.Level - 1) * primaryDamagePerLevel;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 5f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.8f;
    [SerializeField, Min(0)] private float secondaryDamage = 25f;
    [SerializeField, Min(0)] private float secondaryDamagePerLevel = 5f;
    [SerializeField, Min(0)] private float secondaryPushDelay = 0.6f;
    [SerializeField, Min(0)] private float secondaryPushForce = 10f;
    [SerializeField, Min(0)] private float secondaryPushForcePerLevel = 3f;
    [SerializeField] private Vector2 secondaryAttackSphereCast = new(2f, 0f);
    [SerializeField] private AudioClip secondaryAttackSound;
    [SerializeField] private Transform secondaryTornadoPoint;

    public override float SecondaryCooldownDuration => _secondaryCooldown;

    public override float SecondaryDamage => secondaryDamage + (SecondaryUpgrade.Level - 1) * secondaryDamagePerLevel;

    public float SecondaryPushForce => secondaryPushForce + (SecondaryUpgrade.Level - 1) * secondaryPushForcePerLevel;

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
    private void Push(IDamageable damageable, float force, float damage = -1) {
        //Damage damageable
        if (damage > 0) damageable.Damage(damage, DamageType.Ranged, Player);

        //Check if an enemy
        if (damageable is not Enemy) return;

        //Get enemy
        Enemy enemy = damageable as Enemy;

        //Push enemy
        enemy.Push(force * (enemy.transform.position - Player.transform.position));
    }

    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponAction.Secondary, primarySecondaryCooldown);

        //Apply camera knockback
        CameraController.AddKnockback(-transform.forward);

        //Animate
        PlaySound(isPassiveHit ? passiveAttackSound : primaryAttackSound);
        Animator.SetFloat("HitCounter", hitCount % 2);
        Animator.SetTrigger(isPassiveHit ? "ShootStrong" : "Shoot");

        //Shoot
        Projectile projectile = Attack.Throw(bulletPrefab, PrimaryDamage, bulletOrigin);
        if (isPassiveHit) projectile.AddOnHit((damageable) => Push(damageable, PassivePushForce));

        //Next hit
        hitCount = (hitCount + 1) % passiveHit;
        isPassiveHit = hitCount == passiveHit - 1;
        UpdatePassiveValue();
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        //Set cooldown on primary so it can't be used while using secondary
        SetCooldown(WeaponAction.Primary, secondaryPrimaryCooldown);

        //Animate
        Animator.SetTrigger("Push");

        //Wait
        yield return new WaitForSeconds(secondaryPushDelay);

        //Play sound
        PlaySound(secondaryAttackSound);

        //Show VFX
        particleEmitter.PlayOnPosition("airtornado", Vector3.up, Player.Model.position + (secondaryAttackSphereCast.x + secondaryAttackSphereCast.y / 2) * Player.Model.forward);

        //Push enemies back
        Attack.Forward(
            secondaryAttackSphereCast.x,
            secondaryAttackSphereCast.y,
            0,
            true,
            (damageable) => Push(damageable, SecondaryPushForce, SecondaryDamage)
        );
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(WeaponAction.Passive, passiveHit - hitCount - 1);
    }

}
