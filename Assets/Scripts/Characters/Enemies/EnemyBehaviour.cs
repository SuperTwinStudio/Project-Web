using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    //Components
    public EnemyBase Enemy { get; private set; }

    //State
    public EnemyState State { get; private set; }


    //Init
    public void Init(EnemyBase enemy) {
        //Save enemy
        Enemy = enemy;

        //Call init event
        OnInit();
    }

    protected virtual void OnInit() {
        //Init your state & any variables you need
    }

    //States
    public void SetState(EnemyState newState, bool execute = false) {
        //Check if same state
        if (State == newState) return;

        //Notify old state for exit changes
        State?.OnExit();

        //Update state
        State = newState;

        //Check new state for enter changes
        State?.OnEnter();

        //Execute state
        if (execute) State?.Execute();
    }

    //Update
    public void OnUpdate() {
        //Enemy logic loop
        State?.Execute();
    }

    //Health
    public virtual float OnBeforeDamage(float amount, object source, DamageType type = DamageType.None) {
        //Before the enemy was damaged
        return amount;
    }

    public virtual void OnDamage() {
        //Enemy was damaged
    }

    public virtual void OnDeath() {
        //Stop moving
        Enemy.StopMovement();

        //Disable collisions
        Enemy.Collider.enabled = false;

        //Disable script
        Enemy.enabled = false;
    }

}
