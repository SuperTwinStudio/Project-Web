using TMPro;
using UnityEngine;

public class EnemyBase : Character {

    //Player
    protected Player player;

    //Enemy
    [Header("Enemy")]
    [SerializeField] protected GameObject damageIndicatorPrefab;
    [SerializeField] protected new Collider collider;
    [SerializeField] protected new Renderer renderer;
    [SerializeField] protected float viewDistance = 5;

    protected bool playerIsVisible;
    protected float playerDistance;
    protected Vector3 playerLastKnownPosition;

    //Room
    protected Room room = null;


    //State
    protected virtual void Start() {
        //Save player reference
        player = Game.Current.Level.Player;
    }

    protected override void OnUpdate() {
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
    protected override void OnDamageFeedbackStart() {
        base.OnDamageFeedbackStart();

        //Update color
        if (renderer) renderer.material.SetColor("_Color", Color.red);
    }

    protected override void OnDamageFeedbackEnd() {
        base.OnDamageFeedbackEnd();

        //Update color
        if (renderer) renderer.material.SetColor("_Color", Color.white);
    }

    public override bool Damage(float amount, object source, DamageType type = DamageType.None) {
        //Damage
        bool damaged = base.Damage(amount, source, type);

        //Show damage indicator
        if (damaged) {
            var indicator = Instantiate(damageIndicatorPrefab, Top.position + 0.3f * Vector3.up, Quaternion.identity).GetComponent<DamageTextIndicator>();
            string icon = type switch {
                DamageType.Melee => "🔪",
                DamageType.Ranged => "🏹",
                DamageType.Burn => "🔥",
                _ => ""
            };
            indicator.SetText($"{icon}{amount}");
        }

        //Return if damaged
        return damaged;
    }

    protected override void OnDeath(bool instant = false) {
        base.OnDeath(instant);

        //Notify room that enemy was killed
        if (room != null) room.EnemyKilled();

        //Disable collisions
        collider.enabled = false;

        //Disable script
        enabled = false;
    }

    //Room
    public void SetRoom(Room room) {
        this.room = room;
    }

}
