using System.Collections;
using UnityEngine;

public class WeaponSword : Weapon {

    //Effects
    [Header("Effects")]
    [SerializeField] private Effect attackSlowEffect;

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.4f;
    [SerializeField, Min(0)] private float primarySlowDuration = 0.2f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primaryDamage = 30f;
    [SerializeField, Min(0)] private float primaryDamagePerLevel = 10f;
    [SerializeField] private Vector2 primaryAttackSphereCast = new(1f, 0.5f);
    [SerializeField] private AudioClip primaryAttackSound;

    public override float PrimaryCooldownDuration => _primaryCooldown;

    public override float PrimaryDamage => primaryDamage + (PrimaryUpgrade.Level - 1) * primaryDamagePerLevel;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondarySlowDuration = 0.2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.5f;
    [SerializeField, Min(0)] private float secondaryDamage = 50f;
    [SerializeField, Min(0)] private float secondaryDamagePerLevel = 15f;
    [SerializeField, Min(0)] private float secondarySpinRadius = 3f;
    [SerializeField] private AudioClip secondaryAttackSound;

    public override float SecondaryCooldownDuration => _secondaryCooldown;

    public override float SecondaryDamage => secondaryDamage + (SecondaryUpgrade.Level - 1) * secondaryDamagePerLevel;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passiveDamage = 20f;
    [SerializeField, Min(0)] private float passiveDamagePerLevel = 5f;
    [SerializeField, Min(1)] private int passiveHit = 4;
    [SerializeField] private AudioClip passiveAttackSound;

    private bool isPassiveHit = false;
    private int hitCount = 0;

    public override float PassiveCooldown => isPassiveHit ? 0 : 1;

    public override float PassiveDamage => passiveDamage + (PassiveUpgrade.Level - 1) * passiveDamagePerLevel;


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

        //Attack
        MeleeForward(
            primaryAttackSphereCast.x,
            primaryAttackSphereCast.y,
            PrimaryDamage + (isPassiveHit ? PassiveDamage : 0)
        );

        //Animate
        PlaySound(isPassiveHit ? passiveAttackSound : primaryAttackSound);
        Animator.SetFloat("HitCounter", hitCount % 2);
        Animator.SetTrigger(isPassiveHit ? "AttackStrong" : "Attack");

        //Next hit
        hitCount = (hitCount + 1) % passiveHit;
        isPassiveHit = hitCount == passiveHit - 1;
        UpdatePassiveValue();

        //Slow player
        Player.AddEffect(attackSlowEffect, primarySlowDuration);

        //Apply camera knockback
        CameraController.AddKnockback(-transform.forward);
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while using secondary
        SetCooldown(WeaponAction.Primary, secondaryPrimaryCooldown);

        //Attack
        MeleeAround(
            secondarySpinRadius,
            SecondaryDamage
        );

        //Animate
        PlaySound(secondaryAttackSound);
        Animator.SetTrigger("AttackSpin");

        //Slow player
        Player.AddEffect(attackSlowEffect, secondarySlowDuration);

        //Apply camera knockback
        CameraController.AddShake(secondaryPrimaryCooldown);
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(WeaponAction.Passive, passiveHit - hitCount - 1);
    }

}
