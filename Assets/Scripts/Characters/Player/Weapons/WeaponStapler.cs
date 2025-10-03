using System.Collections;
using UnityEngine;

public class WeaponStapler : Weapon {

    //Ammo
    [Header("Ammo")]
    [SerializeField, Min(0)] private GameObject bulletPrefab;
    [SerializeField, Min(0)] private int maxAmmo = 10;
    [SerializeField, Min(0)] private float reloadDuration = 2f;

    private int ammo = 0;
    private bool isReloading = false;

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.2f;

    protected override float PrimaryCooldownDuration => _primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.3f;
    [SerializeField, Min(1)] private int secondaryBurstAmount = 3;
    [SerializeField, Min(0)] private float secondaryBurstDelay = 0.1f;

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passiveMaxDistance = 2;


    //State
    protected override void Start() {
        base.Start();

        //Init ammo
        SetAmmo(maxAmmo);
    }

    private void SetAmmo(int newAmmo) {
        ammo = newAmmo;
        SetValue(WeaponType.Primary, ammo);
    }

    private void Shoot() {
        //No ammo
        if (ammo <= 0) return;

        //Animate
        animator.SetTrigger("Shoot");

        //Shoot

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
        SetCooldown(WeaponType.Primary, reloadDuration);
        SetCooldown(WeaponType.Secondary, reloadDuration);

        //Start reload coroutine
        StartCoroutine(ReloadCoroutine());
    }

    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponType.Secondary, primarySecondaryCooldown);

        //Shoot
        Shoot();

        //Reload
        if (ammo <= 0) Reload();
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while using secondary
        SetCooldown(WeaponType.Primary, secondaryPrimaryCooldown);

        //Shoot burst
        for (int i = 0; i < secondaryBurstAmount; i++) {
            //Shoot
            Shoot();

            //Wait burst delay
            if (ammo > 0) yield return new WaitForSeconds(secondaryBurstDelay);
        }

        //Reload
        if (ammo <= 0) Reload();
    }

}
