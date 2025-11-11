public class DuendeState : EnemyState {

    //Duende
    protected DuendeBehaviour Duende;


    //Constructor
    public DuendeState(EnemyBehaviour behaviour) : base(behaviour) {
        Duende = behaviour as DuendeBehaviour;
    }

}
