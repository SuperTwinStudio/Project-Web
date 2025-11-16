using System;
using System.Collections.Generic;
using Botpa;
using UnityEngine;

public class BeastBehaviour : EnemyBehaviour {

    //Invulnerability Aura
    [Header("Invulnerability Aura")]
    [SerializeField] private GameObject _auraModel;
    [SerializeField] private TriggerDetector _auraDetector;
    [SerializeField, Min(0)] private float _auraDamage = 20.0f;
    [SerializeField, Min(0)] private float _auraPushForce = 25.0f;
    [SerializeField, Min(0)] private float _auraPushCooldown = 0.5f;

    public GameObject AuraModel => _auraModel;
    public TriggerDetector AuraDetector => _auraDetector;
    public float AuraDamage => _auraDamage;
    public float AuraPushForce => _auraPushForce;
    public float AuraCooldown => _auraPushCooldown;

    //Pillars
    [Header("Pillars")]
    [SerializeField] private List<BeastPillar> _pillars = new();
    [SerializeField] private List<Transform> _minionSpawns = new();
    [SerializeField] private GameObject _minionPrefab;

    private event Action<BeastPillar> OnPillarDestroyed;

    public IReadOnlyList<BeastPillar> Pillars => _pillars;
    public IReadOnlyList<Transform> MinionSpawns => _minionSpawns;
    public GameObject MinionPrefab => _minionPrefab;

    //States
    [Header("States")]
    [SerializeField, Min(0)] private float _rageDuration = 2.0f;
    [SerializeField, Min(0)] private float _prechargeDuration = 2.0f;
    [SerializeField, Min(0)] private float _maxChargeDistance = 20.0f;
    [SerializeField, Min(0)] private float _stunDuration = 5.0f;
    [SerializeField] private AudioClip _rageSound;
    [SerializeField] private AudioClip _prechargeSound;
    [SerializeField] private AudioClip _chargeSound;
    [SerializeField] private AudioClip _stunSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _deathSound;

    public float RageDuration => _rageDuration;
    public float PrechargeDuration => _prechargeDuration;
    public float MaxChargeDistance => _maxChargeDistance;
    public float StunDuration => _stunDuration;
    public AudioClip RageSound => _rageSound;
    public AudioClip PrechargeSound => _prechargeSound;
    public AudioClip ChargeSound => _chargeSound;
    public AudioClip StunSound => _stunSound;
    public AudioClip DamageSound => _damageSound;
    public AudioClip DeathSound => _deathSound;


    //Init
    protected override void OnInit() {
        //Disable automatic rotation
        Enemy.SetAutomaticRotation(false);

        //Make enemy invulnerable
        Enemy.IsInvulnerable = true;

        //Init pillars
        foreach (var pillar in _pillars) pillar.Init(this);

        //Go to idle
        SetState(new BeastPillarsState(this));
    }

    //Health
    public override void OnDeath() {
        base.OnDeath();

        //Go to death
        SetState(new BeastDeathState(this));
    }

    //Pillars
    public void AddOnPillarDestroyed(Action<BeastPillar> action) {
        OnPillarDestroyed += action;
    }

    public void RemoveOnPillarDestroyed(Action<BeastPillar> action) {
        OnPillarDestroyed -= action;
    }

    public void NotifyPillarDestroyed(BeastPillar pillar) {
        //Remove from list
        _pillars.Remove(pillar);

        //Call event
        OnPillarDestroyed?.Invoke(pillar);
    }

    //Room
    public override void OnPlayerEnteredRoom() {
        SpawnMinions();
    }

    //Helpers
    public void SpawnMinions() {
        foreach (var spawn in MinionSpawns) Enemy.SpawnEnemy(MinionPrefab, spawn);
    }

}
