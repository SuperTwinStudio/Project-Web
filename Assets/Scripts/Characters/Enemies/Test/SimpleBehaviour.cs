public class SimpleBehaviour : EnemyBehaviour {

    //Init
    protected override void OnInit() {
        //Start in idle state
        SetState(new SimpleIdleState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Set state to death
        SetState(new SimpleDeathState(this));
    }

}
