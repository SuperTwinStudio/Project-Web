public class GoblinState : EnemyState {

    //Duende
    protected GoblinBehaviour Goblin { get; private set; }


    //Constructor
    public GoblinState(EnemyBehaviour behaviour) : base(behaviour) {
        Goblin = behaviour as GoblinBehaviour;
    }

}
