using System.Collections;
using UnityEngine;

public class PlayerClassSword : PlayerClass {

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float primaryDamage = 30;
    [SerializeField, Min(0)] private float primaryRadius = 1;
    [SerializeField, Min(0)] private float primaryDistance = 0;

    public override float PrimaryCooldownDuration => 0.4f;

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float secondaryDamage = 50;

    public override float SecondaryCooldownDuration => 2;

    //Passive
    private int hits = 0;
    private bool isPassiveHit = false;

    private const int PASSIVE_STONG_HIT = 4;
    private const float PASSIVE_DAMAGE_MULT = 2;

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
        bool hit = AtackForward(isPassiveHit ? primaryDamage * PASSIVE_DAMAGE_MULT : primaryDamage, 1, 0);
        
        //Next hit
        hits = (hits + 1) % PASSIVE_STONG_HIT;
        isPassiveHit = hits == PASSIVE_STONG_HIT - 1;
        UpdatePassiveValue();
    }

    //Passive
    private void UpdatePassiveValue() {
        //Update passive value
        SetValue(ClassType.Passive, PASSIVE_STONG_HIT - hits - 1);
    }

}
