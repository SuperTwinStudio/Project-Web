using System.Collections;
using Botpa;
using UnityEngine;

public class WeaponStapler : Weapon {

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.2f;
    [SerializeField, Min(0)] private float primaryDamage = 25f;
    [SerializeField, Min(0)] private float primaryDamagePerLevel = 8f;

    private float PrimaryDamage => primaryDamage + (PrimaryUpgrade.Level - 1) * primaryDamagePerLevel;

    protected override float PrimaryCooldownDuration => _primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float secondaryDamage = 25f;
    [SerializeField, Min(0)] private float secondaryDamagePerLevel = 8f;
    [SerializeField, Min(1)] private int secondaryBurstAmount = 3;
    [SerializeField, Min(0)] private float secondaryBurstDelay = 0.6f;

    private float SecondaryDamage => secondaryDamage + (SecondaryUpgrade.Level - 1) * secondaryDamagePerLevel;

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passiveDamage = 30f;
    [SerializeField, Min(0)] private float passiveDamagePerLevel = 10f;
    [SerializeField] private Vector2 passiveAttackSphereCast = new(1f, 0f);
    [SerializeField] private AudioClip passiveAttackSound;

    private float PassiveDamage => passiveDamage + (PassiveUpgrade.Level - 1) * passiveDamagePerLevel;

    //Reload
    [Header("Reload")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField, Min(0)] private int maxAmmo = 12;
    [SerializeField, Min(0)] private float reloadDuration = 1f;
    [SerializeField] private AudioClip reloadAttackSound;
    [SerializeField] private AudioClip shootAttackSound;

    private readonly Timer reloadTimer = new();
    private int ammo = 0;


    //State
    protected override void OnShow() {
        //Reset ammo
        SetAmmo(maxAmmo);
    }

    private void Update() {
        //Update reload
        if (reloadTimer.counting) {
            //Update value
            SetValue(WeaponAction.Reload, (int) (reloadTimer.percent * 100));
        } else if (reloadTimer.finished) {
            //Reset value
            SetValue(WeaponAction.Reload, -1);
            reloadTimer.Reset();
        }
    }

    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponAction.Secondary, primarySecondaryCooldown);

        //Attack melee (passive)
        bool hit = MeleeForward(
            passiveAttackSphereCast.x, 
            passiveAttackSphereCast.y,
            PassiveDamage
        );

        //Check if melee attack hit something
        if (hit) {
            //Hit something with passive -> Animate attack
            PlaySound(passiveAttackSound);
            Animator.SetTrigger("Attack");
        } else {
            //Didn't hit anything -> Use primary (shoot)
            Shoot(PrimaryDamage);

            //Reload
            if (ammo <= 0) Reload();
        }
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while using secondary
        SetCooldown(WeaponAction.Primary, secondaryPrimaryCooldown);

        //Shoot burst
        for (int i = 0; i < secondaryBurstAmount; i++) {
            //Shoot
            Shoot(SecondaryDamage);

            //Wait burst delay
            if (ammo > 0) yield return new WaitForSeconds(secondaryBurstDelay);
        }

        //Reload
        if (ammo <= 0) Reload();
    }

    //Reloading
    private void SetAmmo(int newAmmo) {
        ammo = newAmmo;
        SetValue(WeaponAction.Passive, ammo);
    }

    private IEnumerator ReloadCoroutine() {
        //Wait
        yield return new WaitForSeconds(reloadDuration);

        //Refill ammo
        SetAmmo(maxAmmo);
    }

    protected override bool OnReload() {
        //Already reloading
        if (IsReloading || ammo >= maxAmmo) return false;

        //Start reload timer
        reloadTimer.Count(reloadDuration);
        SetValue(WeaponAction.Reload, 0);

        //Add reload cooldown to primary and secondary
        SetCooldown(WeaponAction.Primary, reloadDuration);
        SetCooldown(WeaponAction.Secondary, reloadDuration);

        //Play sound
        PlaySound(reloadAttackSound);

        //Start reload coroutine
        StartCoroutine(ReloadCoroutine());
        return true;
    }

    //Helpers
    private void Shoot(float damage) {
        //No ammo
        if (ammo <= 0) return;

        //Shoot
        SpawnProjectile(bulletPrefab, bulletOrigin).GetComponent<Projectile>().Init(Player, damage * Player.DamageMultiplier);

        //Update ammo
        SetAmmo(ammo - 1);

        //Animate
        PlaySound(shootAttackSound);
        Animator.SetTrigger("Attack");

        //Apply camera knockback
        CameraController.AddKnockback(-transform.forward);
    }

    public override float GetWeaponBaseDamage()
    {
        return PrimaryDamage;
    }
}
