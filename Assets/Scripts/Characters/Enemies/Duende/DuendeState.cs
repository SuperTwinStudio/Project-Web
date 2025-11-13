public class DuendeState : EnemyState {

    //Duende
    protected DuendeBehaviour Duende { get; private set; }


    //Constructor
    public DuendeState(EnemyBehaviour behaviour) : base(behaviour) {
        Duende = behaviour as DuendeBehaviour;
    }

}
