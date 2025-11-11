using UnityEngine;
using Botpa;

public class DuendeBehaviour : EnemyBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject projectile;

    //Variables
    [Header("Variables")]
    [SerializeField] private float _minAttackRange = 5.28f;
    [SerializeField] private float _maxAttackRange = 7.33f;
    [SerializeField] private float _evadeRange = 3.65f;
    [SerializeField] private float _spearDamage = 10f;
    [SerializeField] private float _spearCoolDown = 4f;

    public bool OnAttackCooldown { get; private set; } = false;
    
    public float MinAttackRange => _minAttackRange;
    public float MaxAttackRange => _maxAttackRange;
    public float EvadeRange => _evadeRange;
    public float SpearDamage => _spearDamage;
    public float SpearCoolDown => _spearCoolDown;


    //Init
    protected override void OnInit() {
        //Start in idle state
        SetState(new DuendeIdleState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Set state to death
        SetState(new DuendeDeathState(this));
    }

    //Attack
    public void Attack() {
        OnAttackCooldown = true;
        Enemy.Animator.SetTrigger("Attack");    //The actual attack is triggered within the animation
    }

    public void ThrowSpear() {
        Vector3 target = ProyectileIntercept.InterceptPos(hand.position, Enemy.PlayerLastKnownPosition, Enemy.Player.GetVelocity(), 20);
        Vector3 direction = Util.RemoveY((target - hand.position).normalized);
        Enemy.SpawnProjectile(projectile, SpearDamage, direction, hand);
    }

    public void ResetAttackCooldown() {
        OnAttackCooldown = false;
    }

    //Gizmos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MinAttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, MaxAttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, EvadeRange);
    }

}
