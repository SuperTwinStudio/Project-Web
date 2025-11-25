using UnityEngine;

public class MinionBehaviour : EnemyBehaviour {

    //States
    [Header("States")]
    [SerializeField] private float _attackRange = 0.75f;
    [SerializeField] private float _attackRadius = 1f;
    [SerializeField] private float _attackDamage = 10f;

    public float AttackRange => _attackRange;
    public float AttackRadius => _attackRadius;
    public float AttackDamage => _attackDamage;


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
