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

    private float PrimaryDamage => primaryDamage + (PrimaryUpgrade.Level - 1) * primaryDamagePerLevel;

    protected override float PrimaryCooldownDuration => _primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondarySlowDuration = 0.2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.5f;
    [SerializeField, Min(0)] private float secondaryDamage = 50f;
    [SerializeField, Min(0)] private float secondaryDamagePerLevel = 15f;
    [SerializeField] private Vector2 secondaryAttackSphereCast = new(1f, 0f);
    [SerializeField] private AudioClip secondaryAttackSound;

    private float SecondaryDamage => secondaryDamage + (SecondaryUpgrade.Level - 1) * secondaryDamagePerLevel;

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(0)] private float passiveDamage = 4f;
    [SerializeField, Min(0)] private float passiveDamagePerLevel = 2f;
    [SerializeField, Min(0)] private float passiveDuration = 5f;

    private float PassiveDamage => passiveDamage + (PassiveUpgrade.Level - 1) * passiveDamagePerLevel;


    //Primary
    protected override IEnumerator OnUsePrimaryCoroutine() {
        yield return null;

        //Set cooldown on secondary so it can't be used while using primary
        SetCooldown(WeaponAction.Secondary, primarySecondaryCooldown);

        //Attack
        MeleeForward(
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
        MeleeForward(
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
            damageable.Damage(damage, this, DamageType.Melee);
            return;
        }

        //Get character
        Character character = damageable as Character;

        //Apply damage taking effect into account
        damageable.Damage(
            character.TryGetEffect(chinchetaEffect, out float endTimestamp, out int level) ?
                damage + PassiveDamage * level :
                damage,
            this,
            DamageType.Melee
        );

        //Add a new chincheta effect
        character.AddEffect(chinchetaEffect, passiveDuration);
    }

}
