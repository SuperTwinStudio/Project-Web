using System.Collections;
using UnityEngine;

public class WeaponMatch : Weapon {

    [SerializeField] protected Transform tip;
    
    //Effects
    [Header("Effects")]
    [SerializeField] private Effect attackSlowEffect;
    [SerializeField] private Effect burnEffect;

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.4f;
    [SerializeField, Min(0)] private float primarySlowDuration = 0.2f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primaryDamage = 30f;
    [SerializeField, Min(0)] private float primaryDamagePerLevel = 10f;
    [SerializeField] private Vector2 primaryAttackSphereCast = new(0.75f, 1.5f);
    [SerializeField] private AudioClip primaryAttackSound;

    public override float PrimaryCooldownDuration => _primaryCooldown;

    public override float PrimaryDamage => primaryDamage + (PrimaryUpgrade.Level - 1) * primaryDamagePerLevel;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 4.5f;
    [SerializeField, Min(0)] private float secondarySlowDuration = 0.2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.5f;
    [SerializeField, Min(0)] private float secondaryDamage = 40f;
    [SerializeField, Min(0)] private float secondaryDamagePerLevel = 10f;
    [SerializeField, Min(0)] private float secondaryDuration = 2.5f;
    [SerializeField, Min(0)] private float secondaryDurationPerLevel = 0.5f;
    [SerializeField, Min(0)] private float secondaryRadius = 3f;
    [SerializeField] private AudioClip secondaryAttackSound;

    public override float SecondaryCooldownDuration => _secondaryCooldown;

    public override float SecondaryDamage => secondaryDamage + (SecondaryUpgrade.Level - 1) * secondaryDamagePerLevel;

    public float SecondaryBurnDuration => secondaryDuration + (SecondaryUpgrade.Level - 1) * secondaryDurationPerLevel;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passiveDuration = 1f;
    [SerializeField, Min(0)] private float passiveDurationPerLevel = 0.5f;
    [SerializeField, Min(1)] private int passiveHit = 4;
    [SerializeField] private AudioClip passiveAttackSound;

    private bool isPassiveHit = false;
    private int hitCount = 0;

    public float PassiveBurnDuration => passiveDuration + (PassiveUpgrade.Level - 1) * passiveDurationPerLevel;

    public override float PassiveCooldown => isPassiveHit ? 0 : 1;


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

        //Apply camera knockback
        CameraController.AddKnockback(-transform.forward);

        //Slow player
        Player.AddEffect(attackSlowEffect, primarySlowDuration);

        //Animate
        PlaySound(isPassiveHit ? passiveAttackSound : primaryAttackSound);
        Animator.SetTrigger(isPassiveHit ? "AttackStrong" : "Attack");

        //Attack
        Attack.Forward(
            primaryAttackSphereCast.x, 
            primaryAttackSphereCast.y,
            PrimaryDamage,
            true,
            isPassiveHit ? (damageable) => ApplyBurn(damageable, PassiveBurnDuration) : null
        );

        //Next hit
        hitCount = (hitCount + 1) % passiveHit;
        isPassiveHit = hitCount == passiveHit - 1;
        UpdatePassiveValue();
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while using secondary
        SetCooldown(WeaponAction.Primary, secondaryPrimaryCooldown);

        //Apply camera knockback
        CameraController.AddShake();
    
        //Slow player
        Player.AddEffect(attackSlowEffect, secondarySlowDuration);

        //Animate
        Animator.SetTrigger("AttackSlam");

        //Wait for animation
        yield return new WaitForSeconds(0.25f);

        //Play sound
        PlaySound(secondaryAttackSound);

        //Attack
        Attack.Around(
            secondaryRadius,
            SecondaryDamage,
            true,
            (damageable) => ApplyBurn(damageable, SecondaryBurnDuration)
        );
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(WeaponAction.Passive, passiveHit - hitCount - 1);
    }

    //Effects
    private void ApplyBurn(IDamageable damageable, float duration) {
        //Not a character
        if (damageable is not Character) return;

        //Get character
        Character character = damageable as Character;
        character.AddEffect(burnEffect, duration);
    }

    public override void EmitParticle(string name) {
        particleEmitter.PlayOnPosition(name, Vector3.up, tip.position);
    }

}
