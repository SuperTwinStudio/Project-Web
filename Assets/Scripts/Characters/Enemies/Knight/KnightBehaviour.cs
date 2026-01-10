using UnityEngine;

public class KnightBehaviour : EnemyBehaviour {

    //Attack
    [Header("Attack")]
    [SerializeField] private GameObject shield;
    [SerializeField] private float _attackRange = 2.4f;
    [SerializeField] private float _attackRadius = 2f;
    [SerializeField] private float _attackDamage = 20f;

    public bool IsShieldOut { get; private set; } = false;

    public float AttackRange => _attackRange;
    public float AttackRadius => _attackRadius;
    public float AttackDamage => _attackDamage;

    //Sounds
    [Header("Sounds")]
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _deathSound;

    public AudioClip DamageSound => _damageSound;
    public AudioClip DeathSound => _deathSound;


    //Init
    protected override void OnInit() {
        //Go to idle
        SetState(new KnightIdleState(this), false);
    }

    //Health
    public override float OnBeforeDamage(float amount, DamageType type, object source) {
        //Not using shield -> Get damaged
        if (!IsShieldOut) return amount;

        //Can't get character from source -> Get damaged
        if (source is not Character character) return amount;

        //Check direction to block attacks coming from the front
        Vector3 positionSource = character.transform.position;
        Vector3 positionEnemy = Enemy.Model.position;
        Vector3 attackDirection = (positionEnemy - positionSource).normalized;
        return Vector3.Dot(attackDirection, -Enemy.Model.forward) > 0 ? 0 : amount;
    }

    public override void OnDeath() {
        //Go to death
        SetState(new KnightDeathState(this));
    }

    //Shield & Sword
    public void ToggleShield(bool use) {
        IsShieldOut = use;
        shield.SetActive(use);
    }

    public void AttackForward() {
        Enemy.Attack.Forward(AttackRadius, 0, AttackDamage);
    }

    public void FinishAttack() {
        //Return to follow
        SetState(new KnightFollowState(this));
    }

}