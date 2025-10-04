using System.Collections;
using UnityEngine;

public class WeaponSword : Weapon {

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.4f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.2f;
    [SerializeField, Min(0)] private float primaryDamage = 30f;
    [SerializeField, Min(0)] private float primaryAttackRadius = 1f;
    [SerializeField, Min(0)] private float primaryAttackForward = 0f;

    protected override float PrimaryCooldownDuration => _primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.5f;
    [SerializeField, Min(0)] private float secondaryDamage = 50f;
    [SerializeField, Min(0)] private float secondarySpinRadius = 3f;

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(2)] private int passiveHitCount = 4;
    [SerializeField, Min(1)] private float passiveDamageMult = 2f;

    private bool isPassiveHit = false;
    private int hitCount = 0;

    public override float PassiveCooldown => isPassiveHit ? 0 : 1;


    //State
    protected override void Start() {
        base.Start();

        //Init passive
        UpdatePassiveValue();
    }

    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while spinning
        SetCooldown(WeaponType.Secondary, primarySecondaryCooldown);

        //Animate
        animator.SetTrigger(isPassiveHit ? "AttackStrong" : "Attack");

        //Attack
        AtackForward(
            isPassiveHit ? primaryDamage * passiveDamageMult : primaryDamage, 
            primaryAttackRadius, 
            primaryAttackForward
        );
        
        //Next hit
        hitCount = (hitCount + 1) % passiveHitCount;
        isPassiveHit = hitCount == passiveHitCount - 1;
        UpdatePassiveValue();
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while spinning
        SetCooldown(WeaponType.Primary, secondaryPrimaryCooldown);

        //Animate
        animator.SetTrigger("Attack");
        Player.Animator.SetTrigger("SwordSpin");

        //Attack
        AtackAround(
            isPassiveHit ? primaryDamage * passiveDamageMult : primaryDamage, 
            secondarySpinRadius
        );
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(WeaponType.Passive, passiveHitCount - hitCount - 1);
    }

}
