using System.Collections;
using UnityEngine;

public class WeaponStapler : Weapon {

    //Ammo
    [Header("Ammo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField, Min(0)] private int maxAmmo = 12;
    [SerializeField, Min(0)] private float reloadDuration = 1f;

    private int ammo = 0;
    private bool isReloading = false;

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.2f;
    [SerializeField, Min(0)] private float primaryDamage = 10f;
    [SerializeField, Min(0)] private float primaryDamagePerLevel = 5f;

    private float PrimaryDamage => primaryDamage + (PrimaryLevel - 1) * primaryDamagePerLevel;

    protected override float PrimaryCooldownDuration => _primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float secondaryDamage = 10f;
    [SerializeField, Min(0)] private float secondaryDamagePerLevel = 5f;
    [SerializeField, Min(1)] private int secondaryBurstAmount = 3;
    [SerializeField, Min(0)] private float secondaryBurstDelay = 0.6f;

    private float SecondaryDamage => secondaryDamage + (SecondaryLevel - 1) * secondaryDamagePerLevel;

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passiveDamage = 30f;
    [SerializeField, Min(0)] private float passiveDamagePerLevel = 10f;
    [SerializeField, Min(0)] private Vector2 passiveAttackSphereCast = new(1f, 0f);

    private float PassiveDamage => passiveDamage + (PassiveLevel - 1) * passiveDamagePerLevel;


    //State
    protected override void Start() {
        base.Start();

        //Init ammo
        SetAmmo(maxAmmo);
    }

    private void SetAmmo(int newAmmo) {
        ammo = newAmmo;
        SetValue(WeaponAttack.Passive, ammo);
    }

    private void Shoot(float damage) {
        //No ammo
        if (ammo <= 0) return;

        //Animate
        animator.SetTrigger("Shoot");

        //Shoot
        Projectile projectile = SpawnProjectile(bulletPrefab).GetComponent<Projectile>();
        projectile.damage = damage;

        //Update ammo
        SetAmmo(ammo - 1);
    }

    private IEnumerator ReloadCoroutine() {
        //Wait
        yield return new WaitForSeconds(reloadDuration);

        //Refill ammo
        SetAmmo(maxAmmo);

        //Finish
        isReloading = false;
    }

    private void Reload() {
        //Already reloading
        if (isReloading) return;
        isReloading = true;
        
        //Add reload cooldown to primary and secondary
        SetCooldown(WeaponAttack.Primary, reloadDuration);
        SetCooldown(WeaponAttack.Secondary, reloadDuration);

        //Start reload coroutine
        StartCoroutine(ReloadCoroutine());
    }

    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponAttack.Secondary, primarySecondaryCooldown);

        //Attack melee (passive)
        bool hit = AtackForward(
            PassiveDamage, 
            passiveAttackSphereCast.x, 
            passiveAttackSphereCast.y
        );

        //Check if melee attack hit something
        if (hit) {
            //Hit something with passive -> Animate attack
            animator.SetTrigger("Attack");
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
        SetCooldown(WeaponAttack.Primary, secondaryPrimaryCooldown);

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

}
