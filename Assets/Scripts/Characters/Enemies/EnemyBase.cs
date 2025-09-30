using UnityEngine;

public class EnemyBase : Character {

    //Player
    protected Player player;

    //Enemy
    [Header("Enemy")]
    [SerializeField] protected new Collider collider;
    [SerializeField] protected float viewDistance = 5;

    protected bool playerIsVisible;
    protected float playerDistance;
    protected Vector3 playerLastKnownPosition;


    //State
    protected virtual void Start() {
        //Save player reference
        player = Game.Current.Level.Player;
    }

    protected virtual void Update() {
        //Game is paused
        if (Game.IsPaused) return;
        
        //Check if player is visible
        CheckPlayerVisible();
    }

    //Player
    private void CheckPlayerVisible() {
        //Check if player is visible
        playerIsVisible = player.IsVisible(Eyes.position, viewDistance, LayerMask.GetMask("Default"));

        //Save distance
        playerDistance = Vector3.Distance(player.transform.position, transform.position);

        //Save position if visible
        if (playerIsVisible) playerLastKnownPosition = player.transform.position;
    }

    //Health
    protected override void OnDeath(bool instant = false) {
        //Disable collisions
        collider.enabled = false;

        //Disable script
        enabled = false;
    }

}
