public class EnemyState {

    //Enemy
    public EnemyBase Enemy { get; private set; }
    public EnemyBehaviour Behaviour { get; private set; }


    //Constructor
    public EnemyState(EnemyBehaviour behaviour) {
        Enemy = behaviour.Enemy;
        Behaviour = behaviour;
    }

    //Actions
    public virtual void OnEnter() {
        //Called when the state enters
    }

    public virtual void OnExit() {
        //Called when the state exits
    }

    public virtual void Execute() {
        //State logic loop
    }

    public virtual void OnDamage() {
        //Called when the enemy gets damaged
    }

}
