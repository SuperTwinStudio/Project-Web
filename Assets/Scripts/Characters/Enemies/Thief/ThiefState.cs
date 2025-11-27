public class ThiefState : EnemyState {

    //Duende
    protected ThiefBehaviour Thief;


    //Constructor
    public ThiefState(EnemyBehaviour behaviour) : base(behaviour) {
        Thief = behaviour as ThiefBehaviour;
    }

    //Actions
    public override void OnDamage(DamageType type, object source) {
        //Play sound
        if (type != DamageType.Burn) Enemy.PlaySound(Thief.DamageSound);
    }

}
