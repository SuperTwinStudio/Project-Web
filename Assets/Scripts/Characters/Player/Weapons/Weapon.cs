using System;
using System.Collections;
using Botpa;
using UnityEngine;

public enum WeaponType { Primary, Secondary, Passive }

public class Weapon : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] protected Loadout _loadout;

    public Loadout Loadout => _loadout;
    public Player Player => Loadout.Player;

    //Weapon
    [Header("Weapon")]
    [SerializeField] private Item _item;
    [SerializeField] private Sprite _primaryIcon;
    [SerializeField] private Sprite _secondaryIcon;
    [SerializeField] private Sprite _passiveIcon;
    [SerializeField] private GameObject model;
    [SerializeField] protected Animator animator;

    private event Action<WeaponType, int> OnValueChanged;

    public Item Item => _item;
    public Sprite PrimaryIcon => _primaryIcon;
    public Sprite SecondaryIcon => _secondaryIcon;
    public Sprite PassiveIcon => _passiveIcon;

    //Primary
    private readonly Timer primaryTimer = new();

    protected virtual float PrimaryCooldownDuration => 1;

    public virtual bool PrimaryAvailable => !primaryTimer.counting;
    public virtual float PrimaryCooldown => 1 - primaryTimer.percent;       //0 -> No cooldown, 1 -> Full cooldown

    public int PrimaryTier { get; private set; } = 1;
    public virtual int PrimaryUpgradeCostBase => 10;
    public virtual int PrimaryUpgradeCostVariation => 10;
    public virtual int PrimaryUpgradeCost => PrimaryUpgradeCostBase + (PrimaryUpgradeCostVariation * PrimaryTier);

    public int PrimaryValue { get; private set; } = 0;

    //Secondary
    private readonly Timer secondaryTimer = new();

    protected virtual float SecondaryCooldownDuration => 1;

    public virtual bool SecondaryAvailable => !secondaryTimer.counting;
    public virtual float SecondaryCooldown => 1 - secondaryTimer.percent;   //0 -> No cooldown, 1 -> Full cooldown

    public int SecondaryTier { get; private set; } = 1;
    public virtual int SecondaryUpgradeCostBase => 10;
    public virtual int SecondaryUpgradeCostVariation => 10;
    public virtual int SecondaryUpgradeCost => SecondaryUpgradeCostBase + (SecondaryUpgradeCostVariation * SecondaryTier);

    public int SecondaryValue { get; private set; } = 0;

    //Passive
    private readonly Timer passiveTimer = new();

    protected virtual float PassiveCooldownDuration => 1;

    public virtual bool PassiveAvailable => !passiveTimer.counting;
    public virtual float PassiveCooldown => 1 - passiveTimer.percent;       //0 -> No cooldown, 1 -> Full cooldown

    public int PassiveTier { get; private set; } = 1;
    public virtual int PassiveUpgradeCostBase => 10;
    public virtual int PassiveUpgradeCostVariation => 10;
    public virtual int PassiveUpgradeCost => PassiveUpgradeCostBase + (PassiveUpgradeCostVariation * PassiveTier);

    public int PassiveValue { get; private set; } = 0;


    //State
    protected virtual void Start() {
        //Reset timers
        primaryTimer.Count(0);
        secondaryTimer.Count(0);
        passiveTimer.Count(0);
    }

    public void Show(bool show) {
        //Toggle model
        model.SetActive(show);

        //Visible -> Update tiers
        if (show) UpdateTiers();
    }

    //Weapon
    protected void SetCooldown(WeaponType type, float cooldown) {
        //Get timer
        Timer timer = type switch {
            WeaponType.Primary => primaryTimer,
            WeaponType.Secondary => secondaryTimer,
            _ => passiveTimer,
        };

        //Check if timer needs to count or extend
        if (timer.counting) {
            //Already counting -> Check if count is needed
            float remaining = timer.duration - timer.counted;
            if (remaining < cooldown) timer.Extend(cooldown - remaining);
        } else {
            //Not counting -> Count
            timer.Count(cooldown);
        }
    }

    protected void SetValue(WeaponType type, int value) {
        //Update value
        switch (type) {
            case WeaponType.Primary:
                PrimaryValue = value;
                break;
            case WeaponType.Secondary:
                SecondaryValue = value;
                break;
            case WeaponType.Passive:
                PassiveValue = value;
                break;
        }

        //Call event
        OnValueChanged?.Invoke(type, value);
    }

    public void AddOnValueChanged(Action<WeaponType, int> action) {
        OnValueChanged += action;
    }

    public void RemoveOnValueChanged(Action<WeaponType, int> action) {
        OnValueChanged -= action;
    }

    //Upgrades
    private string GetUpgradeName(WeaponType type) {
        return $"{Item.FileName}_{type}";
    }

    private void UpdateTiers() {
        //Update tier values
        PrimaryTier = Loadout.GetUpgrade(GetUpgradeName(WeaponType.Primary));
        SecondaryTier = Loadout.GetUpgrade(GetUpgradeName(WeaponType.Secondary));
        PassiveTier = Loadout.GetUpgrade(GetUpgradeName(WeaponType.Passive));
    }

    public bool Upgrade(WeaponType type) {
        //Get upgrade cost
        int cost = type switch {
            WeaponType.Primary => PrimaryUpgradeCost,
            WeaponType.Secondary => SecondaryUpgradeCost,
            WeaponType.Passive => PassiveUpgradeCost,
            _ => 0
        };

        //Pay for upgrade
        if (!Loadout.PayMoney(cost)) return false;

        //Upgrade
        string name = GetUpgradeName(type);
        switch (type) {
            case WeaponType.Primary:
                PrimaryTier += 1;
                Loadout.SetUpgrade(name, PrimaryTier);
                break;
            case WeaponType.Secondary:
                SecondaryTier += 1;
                Loadout.SetUpgrade(name, SecondaryTier);
                break;
            case WeaponType.Passive:
                PassiveTier += 1;
                Loadout.SetUpgrade(name, PassiveTier);
                break;
        }

        //Success
        return true;
    }

    //Primary
    protected virtual IEnumerator OnUsePrimaryCoroutine() { yield return null; }

    protected virtual void OnUsePrimary() {
        //Start cooldown
        SetCooldown(WeaponType.Primary, PrimaryCooldownDuration);

        //Start use coroutine
        StartCoroutine(OnUsePrimaryCoroutine());
    }

    public bool UsePrimary() {
        if (!PrimaryAvailable) return false;
        OnUsePrimary();
        return true;
    }

    //Secondary
    protected virtual IEnumerator OnUseSecondaryCoroutine() { yield return null; }

    protected virtual void OnUseSecondary() {
        //Start cooldown timer
        SetCooldown(WeaponType.Secondary, SecondaryCooldownDuration);

        //Start use coroutine
        StartCoroutine(OnUseSecondaryCoroutine());
    }

    public bool UseSecondary() {
        if (!SecondaryAvailable) return false;
        OnUseSecondary();
        return true;
    }

    //Actions
    public bool AtackForward(float damage, float radius, float forward) {
        //Casts a sphere of <radius> radius in front of the player and moves it forward <forward> amount to check for collisions
        bool hit = false;

        //Cast attack
        var collisions = Physics.SphereCastAll(transform.position + radius * transform.forward, radius, transform.forward, forward);

        //Check collisions
        foreach (var collision in collisions) {
            //Check if collision is a damageable
            if (!collision.collider.TryGetComponent(out IDamageable damageable)) continue;

            //Ignore player
            if (damageable is Player) continue;

            //Damage
            damageable.Damage(damage);
            hit = true;
        }

        //Return if anything was hit
        return hit;
    }

    public bool AtackAround(float damage, float radius) {
        //Casts a sphere of <radius> radius around the player to check for collisions
        bool hit = false;

        //Cast attack
        var collisions = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0);

        //Check collisions
        foreach (var collision in collisions) {
            //Check if collision is a damageable
            if (!collision.collider.TryGetComponent(out IDamageable damageable)) continue;

            //Ignore player
            if (damageable is Player) continue;

            //Damage
            damageable.Damage(damage);
            hit = true;
        }

        //Return if anything was hit
        return hit;
    }

    public bool SpawnProjectile(GameObject prefab) { return false; }

}