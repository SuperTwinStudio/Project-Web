using UnityEngine;

public class MinionBehaviour : EnemyBehaviour {

    //Values
    [Header("Values")]
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackRadius = 1f;
    [SerializeField] private float _attackDamage = 10f;

    public float AttackRange => _attackRange;
    public float AttackRadius => _attackRadius;
    public float AttackDamage => _attackDamage;

    //Sounds
    [Header("Sounds")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _deathSound;

    public AudioClip AttackSound => _attackSound;
    public AudioClip DamageSound => _damageSound;
    public AudioClip DeathSound => _deathSound;


    //Init
    protected override void OnInit() {
        //Go to idle
        SetState(new MinionIdleState(this));
    }

    //Health
    public override void OnDeath() {
        //Go to death
        SetState(new MinionDeathState(this));
    }

}
