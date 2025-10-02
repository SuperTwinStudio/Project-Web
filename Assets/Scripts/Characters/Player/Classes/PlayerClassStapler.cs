using System.Collections;
using UnityEngine;

public class PlayerClassStapler : PlayerClass {

    //Ammo
    [Header("Ammo")]
    [SerializeField, Min(0)] private int maxAmmo = 10;
    [SerializeField, Min(0)] private float reloadDuration = 2f;

    private int ammo = 10;
    private bool isReloading = false;

    //Primary
    public override float PrimaryCooldownDuration => 0.3f;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(1)] private int secondaryBurst = 3;
    [SerializeField, Min(0)] private float secondaryBurstDelay = 0.1f;

    public override float SecondaryCooldownDuration => 2;

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
        SetValue(ClassType.Primary, ammo);
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
        SetCooldown(ClassType.Primary, reloadDuration);
        SetCooldown(ClassType.Secondary, reloadDuration);

        //Start reload coroutine
        StartCoroutine(ReloadCoroutine());
    }

    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Shoot
        Shoot();

        //Reload
        if (ammo <= 0) Reload();
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while using secondary
        SetCooldown(ClassType.Primary, PrimaryCooldownDuration);

        //Shoot burst
        for (int i = 0; i < secondaryBurst; i++) {
            //Shoot
            Shoot();

            //Wait burst delay
            if (ammo > 0) yield return new WaitForSeconds(secondaryBurstDelay);
        }

        //Reload
        if (ammo <= 0) Reload();
    }

}
