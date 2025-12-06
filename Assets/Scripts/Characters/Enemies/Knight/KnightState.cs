public class KnightState : EnemyState {

    //Knight
    protected KnightBehaviour Knight { get; private set; }


    //Constructor
    public KnightState(EnemyBehaviour behaviour) : base(behaviour) {
        Knight = behaviour as KnightBehaviour;
    }

    //Actions
    public override void OnDamage(DamageType type, object source) {
        //Play sound
        Enemy.PlaySound(Knight.DamageSound);
    }

}
