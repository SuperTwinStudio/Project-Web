public class LadronzueloBehaviour : EnemyBehaviour {

    //Init
    protected override void OnInit() {
        //Start in idle state
        SetState(new IdleState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Set state to death
        SetState(new DeathState(this));
    }

}
