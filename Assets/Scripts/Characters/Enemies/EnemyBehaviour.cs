using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private EnemyBase _enemy;

    public EnemyBase Enemy => _enemy;


    //State
    public void OnStart() {
        //Start any variables you need
    }

    public void OnUpdate() {
        //Enemy logic loop
        if (Enemy.PlayerIsVisible) Enemy.MoveTowards(Enemy.PlayerLastKnownPosition);
    }

    //Health
    public void OnDamage() {
        //Enemy was damaged
    }

    public void OnDeath() {
        //Notify room that enemy was killed
        if (Enemy.Room) Enemy.Room.EnemyKilled();

        //Stop moving
        Enemy.StopMovement();

        //Disable collisions
        Enemy.Collider.enabled = false;

        //Disable script
        Enemy.enabled = false;
    }

}
