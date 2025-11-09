using UnityEngine;
public class DuendeBehaviour : EnemyBehaviour
{

    [Header("Components")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform hand;

    [Header("Variables")]
    [SerializeField] public float minAttackRange = 2.5f;
    [SerializeField] public float maxAttackRange = 5f;
    [SerializeField] public float evadeRange = 2f;
    [SerializeField] public float spearDamage = 10;
    [SerializeField] public float spearCoolDown = 2f;

    //Init
    protected override void OnInit() {
        //Start in idle state
        SetState(new DuendeIdleState(this));
    }

    //Health
    public override void OnDeath()
    {
        base.OnDeath();

        //Set state to death
        SetState(new SimpleDeathState(this));
    }

    public void ThrowSpear()
    {
        Enemy.SpawnProjectile(projectile, spearDamage, hand);
    }


    private void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minAttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxAttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, evadeRange);

    }

}
