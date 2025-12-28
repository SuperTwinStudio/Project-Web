using System;
using UnityEngine;

public class ThiefBehaviour : EnemyBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private GameObject hat;
    [SerializeField] private Effect _stealEffect;
    [SerializeField] private Effect _fleeEffect;

    private Loadout Loadout => Enemy.Player.Loadout;

    public Effect StealEffect => _stealEffect;
    public Effect FleeEffect => _fleeEffect;

    //Values
    [Header("Values")]
    [SerializeField] private float _interactRange = 1.5f;
    [SerializeField] private float _attackRadius = 1f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private int _stealAmount = 30;
    [SerializeField] private float _maxFleeDistance = 25;

    public int StolenAmount { get; private set; }
    public bool CanSteal { get; private set; }

    public float InteractRange => _interactRange;
    public float AttackRadius => _attackRadius;
    public float AttackDamage => _attackDamage;
    public bool PlayerHasGold => Loadout.Gold > 0;
    public bool HasStolen => StolenAmount > 0;
    public int StealAmount => _stealAmount;
    public float MaxFleeDistance => _maxFleeDistance;

    //Sounds
    [Header("Sounds")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _fleeSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _deathSound;

    public AudioClip AttackSound => _attackSound;
    public AudioClip FleeSound => _fleeSound;
    public AudioClip DamageSound => _damageSound;
    public AudioClip DeathSound => _deathSound;


    //Init
    protected override void OnInit() {
        //Add update can steal event
        Enemy.Room.AddOnEnemiesChanged(UpdateCanSteal);
        UpdateCanSteal();

        //Go to idle
        SetState(new ThiefIdleState(this), false);
    }

    //Health
    public override void OnDeath() {
        //Remove update can steal event
        Enemy.Room.AddOnEnemiesChanged(UpdateCanSteal);

        //Go to death
        SetState(new ThiefDeathState(this));
    }

    //Helpers
    private void UpdateCanSteal() {
        //Update using basic checks
        CanSteal = PlayerHasGold && !HasStolen;

        //Check if basic checks were passed
        if (CanSteal) {
            //Result variables
            bool otherEnemyTypesPresent = false;
            int thiefsCount = 0;

            //Check room for enemies
            foreach (Enemy enemy in Enemy.Room.Enemies) {
                if (enemy.Behaviour is ThiefBehaviour)
                    thiefsCount++;
                else
                    otherEnemyTypesPresent = true;
            }

            //Update using present enemy types
            CanSteal = otherEnemyTypesPresent && thiefsCount == 1;
        }

        //Update visuals
        hat.SetActive(CanSteal);
    }

    public bool StealGoldFromPlayer() {
        //Check if can steal
        if (!PlayerHasGold) return false;

        //Try steal gold
        int gold = Math.Min(Loadout.Gold, StealAmount);
        if (!Loadout.SpendGold(gold)) return false;

        //Add gold to stolen amount
        StolenAmount += gold;

        //Add gold stolen effect
        Enemy.AddEffect(StealEffect, float.PositiveInfinity, gold);
        return true;
    }

    public void ReturnGoldToPlayer() {
        //Nothing to return
        if (StolenAmount <= 0) return;

        //Return gold to player
        Loadout.AddGold(StolenAmount);
        StolenAmount = 0;

        //Remove gold stolen effect
        Enemy.RemoveEffect(StealEffect, StolenAmount);
    }

}
