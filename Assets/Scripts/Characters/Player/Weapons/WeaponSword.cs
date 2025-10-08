using System.Collections;
using UnityEngine;

public class WeaponSword : Weapon {

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.4f;
    [SerializeField, Min(0)] private float primarySlowDuration = 0.2f;
    [SerializeField, Min(0)] private float primarySecondaryCooldown = 0.2f;
    [SerializeField, Min(0)] private float primaryDamage = 30f;
    [SerializeField, Min(0)] private float primaryDamagePerTier = 10f;
    [SerializeField, Min(0)] private Vector2 primaryAttackSphereCast = new(1f, 0f);

    private float PrimaryDamage => primaryDamage + (PrimaryLevel - 1) * primaryDamagePerTier;

    protected override float PrimaryCooldownDuration => _primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondarySlowDuration = 0.2f;
    [SerializeField, Min(0)] private float secondaryPrimaryCooldown = 0.5f;
    [SerializeField, Min(0)] private float secondaryDamage = 50f;
    [SerializeField, Min(0)] private float secondaryDamagePerTier = 15f;
    [SerializeField, Min(0)] private float secondarySpinRadius = 3f;

    private float SecondaryDamage => secondaryDamage + (SecondaryLevel - 1) * secondaryDamagePerTier;

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(2)] private int passiveHit = 4;
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
        SetCooldown(WeaponAttack.Secondary, primarySecondaryCooldown);

        //Slow player
        Player.AddEffect(Effect.GetFromName("AttackSlow"), primarySlowDuration);

        //Animate
        animator.SetTrigger(isPassiveHit ? "AttackStrong" : "Attack");

        //Attack
        AtackForward(
            PrimaryDamage * (isPassiveHit ? passiveDamageMult : 1), 
            primaryAttackSphereCast.x, 
            primaryAttackSphereCast.y
        );
        
        //Next hit
        hitCount = (hitCount + 1) % passiveHit;
        isPassiveHit = hitCount == passiveHit - 1;
        UpdatePassiveValue();
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while spinning
        SetCooldown(WeaponAttack.Primary, secondaryPrimaryCooldown);

        //Slow player
        Player.AddEffect(Effect.GetFromName("AttackSlow"), secondarySlowDuration);

        //Animate
        animator.SetTrigger("Attack");
        Player.Animator.SetTrigger("SwordSpin");

        //Attack
        AtackAround(
            SecondaryDamage, 
            secondarySpinRadius
        );
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(WeaponAttack.Passive, passiveHit - hitCount - 1);
    }

}
