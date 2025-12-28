using System;
using TMPro;
using UnityEngine;

public class ThiefBehaviour : EnemyBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private TMP_Text stolenAmountText;
    [SerializeField] private Effect _fleeEffect;

    private Loadout Loadout => Enemy.Player.Loadout;

    public Effect FleeEffect => _fleeEffect;

    //Values
    [Header("Values")]
    [SerializeField] private float _interactRange = 1.5f;
    [SerializeField] private float _attackRadius = 1f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private int _stealAmount = 30;
    [SerializeField] private float _maxFleeDistance = 25;

    public int StolenAmount { get; private set; }

    public float InteractRange => _interactRange;
    public float AttackRadius => _attackRadius;
    public float AttackDamage => _attackDamage;
    public bool PlayerHasGold => Loadout.Gold > 0;
    public int StealAmount => _stealAmount;
    public bool HasStolen => StolenAmount > 0;
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
        //Go to idle
        SetState(new ThiefIdleState(this), false);
    }

    //Health
    public override void OnDeath() {
        //Go to death
        SetState(new ThiefDeathState(this));
    }

    //Helpers
    public void UpdateGoldText() {
        if (StolenAmount <= 0)
            //Hide stolen amount
            stolenAmountText.SetText("");
        else
            //Show stolen amount
            stolenAmountText.SetText($"{StolenAmount}G");
    }

    public bool CheckIfAllowedToSteal() {
        //Player has no gold
        if (!PlayerHasGold) return false;

        //Already stole gold
        if (HasStolen) return false;

        //Result variables
        bool otherTypes = false;
        int thiefs = 0;

        //Check room for enemies
        foreach (Enemy enemy in Enemy.Room.Enemies) {
            if (enemy.Behaviour is ThiefBehaviour)
                thiefs++;
            else
                otherTypes = true;
        }

        //Check if allowed to steal
        return otherTypes && thiefs == 1;
    }

    public bool StealGoldFromPlayer() {
        //Check if can steal
        if (!PlayerHasGold) return false;

        //Try steal gold
        int gold = Math.Min(Loadout.Gold, StealAmount);
        if (!Loadout.SpendGold(gold)) return false;

        //Add gold to stolen amount
        StolenAmount += gold;
        UpdateGoldText();
        return true;
    }

    public void ReturnGoldToPlayer() {
        //Nothing to return
        if (StolenAmount <= 0) return;

        //Return gold to player
        Loadout.AddGold(StolenAmount);
        StolenAmount = 0;
        UpdateGoldText();
    }

}
