public class LadronzueloDeathState : SimpleDeathState {

    //Constructor
    public LadronzueloDeathState(EnemyBehaviour behaviour) : base(behaviour) {}

    //Actions
    public override void OnEnter() {
        //Return money
        (Behaviour as LadronzueloBehaviour).ReturnGoldToPlayer();

        //Die
        base.OnEnter();
    }

}
