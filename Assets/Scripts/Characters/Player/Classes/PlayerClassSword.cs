using System.Collections;
using UnityEngine;

public class PlayerClassSword : PlayerClass {

    //Primary
    [Header("Primary")]
    [SerializeField, Min(0)] private float primaryDamage = 30;
    [SerializeField, Min(0)] private float primaryRadius = 1;
    [SerializeField, Min(0)] private float primaryDistance = 0;
    [SerializeField, Min(0)] private float primaryDelayBefore = 0.2f;   //Delay before damage
    [SerializeField, Min(0)] private float primaryDelayAfter = 0.2f;    //Delay after damage

    //Secondary
    [Header("Secondary")]
    [SerializeField, Min(0)] private float secondaryDamage = 50;

    //Passive
    private int hits = 0;

    private const int PASSIVE_STONG_HIT = 4;
    private const float PASSIVE_DAMAGE_MULT = 2;


    //Attacks
    protected override IEnumerator PrimaryAttack() {
        //Start animation & wait
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(primaryDelayBefore);

        //Passive
        bool isPassiveHit = hits == PASSIVE_STONG_HIT - 1;

        //Attack
        bool hit = AtackForward(isPassiveHit ? primaryDamage * PASSIVE_DAMAGE_MULT : primaryDamage, 1, 0);
        if (hit) hits = (hits + 1) % PASSIVE_STONG_HIT;

        //Wait
        yield return new WaitForSeconds(primaryDelayAfter);

        //Finish attack
        CanUsePrimary = true;
    }

}
