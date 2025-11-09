using UnityEngine;

public class KnightBehaviour : EnemyBehaviour
{
  //Init
    protected override void OnInit() {
        //Start in idle state
        SetState(new KnightIdleState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Set state to death
        SetState(new SimpleDeathState(this));
    }

}