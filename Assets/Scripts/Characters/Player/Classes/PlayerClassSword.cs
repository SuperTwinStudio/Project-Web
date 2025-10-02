using System.Collections;
using UnityEngine;

public class PlayerClassSword : PlayerClass {

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float _primaryCooldown = 0.4f;
    [SerializeField, Min(0)] private float primaryDamage = 30;
    [SerializeField, Min(0)] private float primaryAttackRadius = 1;
    [SerializeField, Min(0)] private float primaryAttackDistance = 0;

    protected override float PrimaryCooldownDuration => _primaryCooldown;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float _secondaryCooldown = 2f;
    [SerializeField, Min(0)] private float secondaryDamage = 50;

    protected override float SecondaryCooldownDuration => _secondaryCooldown;

    //Passive
    [Header("Passive")]
    [SerializeField, Min(2)] private int passiveStrongHit = 4;
    [SerializeField, Min(1)] private float passiveDamageMult = 2f;

    private bool isPassiveHit = false;
    private int hits = 0;

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

        //Animate
        animator.SetTrigger(isPassiveHit ? "AttackStrong" : "Attack");

        //Attack
        bool hit = AtackForward(
            isPassiveHit ? primaryDamage * passiveDamageMult : primaryDamage, 
            primaryAttackRadius, 
            primaryAttackDistance
        );
        
        //Next hit
        hits = (hits + 1) % passiveStrongHit;
        isPassiveHit = hits == passiveStrongHit - 1;
        UpdatePassiveValue();
    }

    //Secondary
    protected override IEnumerator OnUseSecondaryCoroutine() {
        yield return null;

        //Set cooldown on primary so it can't be used while using secondary
        SetCooldown(ClassType.Primary, PrimaryCooldownDuration);
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(ClassType.Passive, passiveStrongHit - hits - 1);
    }

}
