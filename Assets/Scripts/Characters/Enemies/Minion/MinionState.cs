public class MinionState : EnemyState {

    //Minion
    protected MinionBehaviour Minion { get; private set; }


    //Constructor
    public MinionState(EnemyBehaviour behaviour) : base(behaviour) {
        Minion = behaviour as MinionBehaviour;
    }

}
