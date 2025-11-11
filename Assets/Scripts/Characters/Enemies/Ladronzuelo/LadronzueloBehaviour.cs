using UnityEngine;

public class LadronzueloBehaviour : EnemyBehaviour {

    //Attack
    [Header("Attack")]
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackDamage = 10f;

    public float AttackRange => _attackRange;
    public float AttackDamage => _attackDamage;

    //Steal
    [Header("Steal")]
    [SerializeField] private float _stealRange = 1.5f;
    [SerializeField] private int _stealAmount = 10;

    public float StealRange => _stealRange;
    public int StealAmount => _stealAmount;


    //Init
    protected override void OnInit() {
        //Start in idle state
        SetState(new LadronzueloIdleState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Set state to death
        SetState(new SimpleDeathState(this));
    }

}
