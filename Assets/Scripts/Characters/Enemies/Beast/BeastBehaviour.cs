using System.Collections.Generic;
using Botpa;
using UnityEngine;

public class BeastBehaviour : EnemyBehaviour {

    //Invulnerability Aura
    [Header("Invulnerability Aura")]
    [SerializeField] private TriggerDetector _auraDetector;
    [SerializeField, Min(0)] private float _auraDamage = 20.0f;
    [SerializeField, Min(0)] private float _auraPushForce = 25.0f;
    [SerializeField, Min(0)] private float _auraPushCooldown = 0.5f;

    public TriggerDetector AuraDetector => _auraDetector;
    public float AuraDamage => _auraDamage;
    public float AuraPushForce => _auraPushForce;
    public float AuraCooldown => _auraPushCooldown;

    //Pillars
    [Header("Pillars")]
    [SerializeField] private List<BeastPillar> pillars = new();

    //States
    [Header("States")]
    [SerializeField, Min(0)] private float _rageDuration = 2.0f;
    [SerializeField, Min(0)] private float _prechargeDuration = 2.0f;
    [SerializeField, Min(0)] private float _maxChargeDistance = 20.0f;
    [SerializeField, Min(0)] private float _stunDuration = 5.0f;

    public float RageDuration => _rageDuration;
    public float PrechargeDuration => _prechargeDuration;
    public float MaxChargeDistance => _maxChargeDistance;
    public float StunDuration => _stunDuration;


    //Init
    protected override void OnInit() {
        //Disable automatic rotation
        Enemy.SetAutomaticRotation(false);

        //Make enemy invulnerable
        Enemy.IsInvulnerable = true;

        //Init pillars
        foreach (var pillar in pillars) pillar.Init(this);

        //Start in idle state
        SetState(new BeastPillarsState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Set state to death
        SetState(new BeastDeathState(this));
    }

    //Pillars
    public void OnPillarDestroyed(BeastPillar pillar) {
        //Remove from list
        pillars.Remove(pillar);

        //Check remaining pillars
        if (pillars.Count >= 2) {
            //Still 2+ columns -> Spawn enemies on the broken pillar
            Debug.Log("Spawn enemies");
        } else if (pillars.Count == 1) {
            //Only 1 column remaining -> Destroy it
            BeastPillar remainingPillar = pillars[0];
            remainingPillar.Damage(remainingPillar.Health, Enemy, DamageType.None);
            pillars.Clear();

            //Start rage phase
            SetState(new BeastRageState(this));
        }
    }

}
