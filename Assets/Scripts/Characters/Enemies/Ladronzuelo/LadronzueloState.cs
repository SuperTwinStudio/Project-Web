public class LadronzueloState : EnemyState {

    //Duende
    protected LadronzueloBehaviour Ladronzuelo;


    //Constructor
    public LadronzueloState(EnemyBehaviour behaviour) : base(behaviour) {
        Ladronzuelo = behaviour as LadronzueloBehaviour;
    }

}
