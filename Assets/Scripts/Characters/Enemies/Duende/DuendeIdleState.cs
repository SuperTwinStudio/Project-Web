public class DuendeIdleState : EnemyState {

    private const float MIN_ATTACK_RANGE = 2.5f;
    private const float MAX_ATTACK_RANGE = 5f;


    //Constructor
    public DuendeIdleState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnEnter() {
        //Called when the state enters
    }

    public override void OnExit() {
        //Called when the state exits
    }

    public override void Execute() {
        //Check if player is visible
        if (!Enemy.PlayerIsVisible) return;

        if (Enemy.PlayerDistance > MAX_ATTACK_RANGE)
        {
            //Player too far -> follow him
            Behaviour.SetState(new DuendeFollowState(Behaviour));
        }
        else if (Enemy.PlayerDistance > MIN_ATTACK_RANGE)
        {
            //Player within attack range -> Attack him
            Behaviour.SetState(new DuendeAttackState(Behaviour));
        }
        else
        {
            //Player too close -> Evade
            Behaviour.SetState(new DuendeEvadeState(Behaviour));
        }
        
    }

}
