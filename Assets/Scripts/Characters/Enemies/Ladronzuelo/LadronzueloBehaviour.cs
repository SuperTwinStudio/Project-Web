using System;
using TMPro;
using UnityEngine;

public class LadronzueloBehaviour : EnemyBehaviour {

    //Components
    private Loadout Loadout => Enemy.Player.Loadout;

    //Interact
    [Header("Interact")]
    [SerializeField] private float _interactRange = 1.5f;

    public float InteractRange => _interactRange;

    //Attack
    [Header("Attack")]
    [SerializeField] private float _attackDamage = 10f;

    public float AttackDamage => _attackDamage;

    //Stealing
    [Header("Stealing")]
    [SerializeField] private TMP_Text stolenAmountText;
    [SerializeField] private int _stealAmount = 10;

    private int stolenAmount = 0;

    public bool PlayerHasGold => Loadout.Gold > 0;
    public int StealAmount => _stealAmount;


    //Init
    protected override void OnInit() {
        //Reset stolen amount text
        stolenAmountText.SetText("");

        //Start in idle state
        SetState(new LadronzueloIdleState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Set state to death
        SetState(new LadronzueloDeathState(this));
    }

    //Helpers
    public bool CheckIfAllowedToSteal() {
        //Player has no gold
        if (!PlayerHasGold) return false;

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
        stolenAmount += gold;
        stolenAmountText.SetText($"{stolenAmount}G");
        return true;
    }

    public void ReturnGoldToPlayer() {
        //Nothing to return
        if (stolenAmount <= 0) return;

        //Return gold to player
        Loadout.AddGold(stolenAmount);
        stolenAmountText.SetText("");
    }

}
