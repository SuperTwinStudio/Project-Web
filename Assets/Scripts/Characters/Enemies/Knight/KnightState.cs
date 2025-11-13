public class KnightState : EnemyState {

    //Knight
    protected KnightBehaviour Knight { get; private set; }


    //Constructor
    public KnightState(EnemyBehaviour behaviour) : base(behaviour) {
        Knight = behaviour as KnightBehaviour;
    }

}
