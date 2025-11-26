public class MinionState : EnemyState {

    //Minion
    protected MinionBehaviour Minion { get; private set; }


    //Constructor
    public MinionState(EnemyBehaviour behaviour) : base(behaviour) {
        Minion = behaviour as MinionBehaviour;
    }

    //Actions
    public override void OnDamage(DamageType type, object source) {
        //Play sound
        if (type != DamageType.Burn) Enemy.PlaySound(Minion.DamageSound);
    }

}
