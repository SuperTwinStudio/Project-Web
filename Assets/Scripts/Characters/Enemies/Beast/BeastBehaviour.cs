public class BeastBehaviour : EnemyBehaviour {

    //Init
    protected override void OnInit() {
        //Disable automatic rotation
        Enemy.SetAutomaticRotation(true);

        //Start in idle state
        SetState(new SimpleIdleState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Set state to death
        SetState(new BeastRageState(this));
    }

}
