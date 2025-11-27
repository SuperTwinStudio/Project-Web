using UnityEngine;

public class KnightBehaviour : EnemyBehaviour {

    //Attack
    [Header("Attack")]
    [SerializeField] private float _attackRange = 1.3f;
    [SerializeField] private float _attackRadius = 1.6f;
    [SerializeField] private float _attackDamage = 20f;

    public bool IsShieldOut { get; private set; } = false;

    public float AttackRange => _attackRange;
    public float AttackRadius => _attackRadius;
    public float AttackDamage => _attackDamage;

    //Stun
    [Header("Stun")]
    [SerializeField] private float _stunDuration = 2.5f;

    public float StunDuration => _stunDuration;


    //Init
    protected override void OnInit() {
        //Go to idle
        SetState(new KnightIdleState(this));
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

    //Shield
    public void ToggleShield(bool use) {
        IsShieldOut = use;
    }

}