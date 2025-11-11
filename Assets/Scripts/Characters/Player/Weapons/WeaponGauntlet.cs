using System.Collections;
using UnityEngine;

public class WeaponGauntlet : Weapon {

    //Temp
    [Header("Temp")]
    [SerializeField] private Animator _animator;

    protected override Animator Animator => _animator;

    //Effects
    [Header("Effects")]
    [SerializeField] private Effect attackSlowEffect;
    [SerializeField] private Effect chinchetaEffect;

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.4f;
    [SerializeField, Min(0)] private float primarySlowDuration = 0.2f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.3f;
    [SerializeField, Min(0)] private float primaryDamage = 20f;
    [SerializeField, Min(0)] private float primaryDamagePerLevel = 5f;
    [SerializeField] private Vector2 primaryAttackSphereCast = new(1f, 0f);
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
    [SerializeField] private Vector2 secondaryAttackSphereCast = new(1f, 0f);
    [SerializeField] private AudioClip secondaryAttackSound;

    public override float SecondaryCooldownDuration => _secondaryCooldown;

    public override float SecondaryDamage => secondaryDamage + (SecondaryUpgrade.Level - 1) * secondaryDamagePerLevel;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passiveDamage = 4f;
    [SerializeField, Min(0)] private float passiveDamagePerLevel = 2f;
    [SerializeField, Min(0)] private float passiveDuration = 5f;

    public override float PassiveDamage => passiveDamage + (PassiveUpgrade.Level - 1) * passiveDamagePerLevel;


    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponAction.Secondary, primarySecondaryCooldown);

        //Attack
        Attack.Forward(
            primaryAttackSphereCast.x,
            primaryAttackSphereCast.y,
            0,
            (damageable) => ApplyChincheta(damageable, PrimaryDamage)
        );

        //Animate
        PlaySound(primaryAttackSound);
        Animator.SetTrigger("Attack");

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
        Attack.Forward(
            secondaryAttackSphereCast.x,
            secondaryAttackSphereCast.y,
            0,
            (damageable) => ApplyChincheta(damageable, SecondaryDamage)
        );

        //Animate
        PlaySound(secondaryAttackSound);
        Animator.SetTrigger("AttackDown");

        //Slow player
        Player.AddEffect(attackSlowEffect, secondarySlowDuration);

        //Apply camera knockback
        CameraController.AddShake(secondaryPrimaryCooldown / 2);
    }

    //Passive
    private void ApplyChincheta(IDamageable damageable, float damage) {
        //Check type
        if (damageable is not Character) {
            //Not a character -> Default damage
            damageable.Damage(CalculateDamage(damage), this, DamageType.Melee);
            return;
        }

        //Get character
        Character character = damageable as Character;

        //Apply damage taking effect into account
        damageable.Damage(
            character.TryGetEffect(chinchetaEffect, out float endTimestamp, out int level) ?
                CalculateDamage(damage + PassiveDamage * level) :
                CalculateDamage(damage),
            this,
            DamageType.Melee
        );

        //Add a new chincheta effect
        character.AddEffect(chinchetaEffect, passiveDuration);
    }

}
