using System;
using TMPro;
using UnityEngine;

public class LadronzueloBehaviour : EnemyBehaviour {

    //Components
    private Loadout Loadout => Enemy.Player.Loadout;

    //States
    [Header("States")]
    [SerializeField] private TMP_Text stolenAmountText;
    [SerializeField] private Effect _fleeEffect;
    [SerializeField] private float _interactRange = 1.5f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private int _stealAmount = 50;
    [SerializeField] private float _maxFleeDistance = 25;

    public int StolenAmount { get; private set; }

    public Effect FleeEffect => _fleeEffect;
    public float InteractRange => _interactRange;
    public float AttackDamage => _attackDamage;
    public bool PlayerHasGold => Loadout.Gold > 0;
    public int StealAmount => _stealAmount;
    public bool HasStolen => StolenAmount > 0;
    public float MaxFleeDistance => _maxFleeDistance;


    //Init
    protected override void OnInit() {
        //Go to idle
        SetState(new LadronzueloIdleState(this));
    }

    //Health
    public override void OnDeath() {
        //Go to death
        SetState(new LadronzueloDeathState(this));
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
        int ladronzuelos = 0;

        //Check if enemy is in a room
        if (Enemy.Room) {
            //Has room -> Check room for enemies
            foreach (EnemyBase enemy in Enemy.Room.Enemies)
                if (enemy.Behaviour is LadronzueloBehaviour)
                    ladronzuelos++;
                else
                    otherTypes = true;
        } else {
            //No room -> Check enemy gameobjects (slower)
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
                if (enemy.GetComponent<LadronzueloBehaviour>())
                    ladronzuelos++;
                else
                    otherTypes = true;
            }
        }

        //Check if allowed to steal
        return otherTypes && ladronzuelos == 1;
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
