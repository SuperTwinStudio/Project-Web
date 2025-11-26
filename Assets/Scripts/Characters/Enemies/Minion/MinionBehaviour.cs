using UnityEngine;

public class MinionBehaviour : EnemyBehaviour {

    //States
    [Header("States")]
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackRadius = 1.25f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _deathSound;

    public float AttackRange => _attackRange;
    public float AttackRadius => _attackRadius;
    public float AttackDamage => _attackDamage;
    public AudioClip AttackSound => _attackSound;
    public AudioClip DamageSound => _damageSound;
    public AudioClip DeathSound => _deathSound;


    //Init
    protected override void OnInit() {
        //Go to idle
        SetState(new MinionIdleState(this));
    }

    //Health
    public override void OnDamage(DamageType type, object source) {
        base.OnDamage(type, source);

        //Play sound
        Enemy.PlaySound(DamageSound);
    }

    public override void OnDeath() {
        //Go to death
        SetState(new MinionDeathState(this));
    }

}
