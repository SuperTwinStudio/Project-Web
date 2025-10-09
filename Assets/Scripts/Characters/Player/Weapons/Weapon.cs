using System;
using System.Collections;
using Botpa;
using UnityEngine;

public enum WeaponAttack { Primary, Secondary, Passive }

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

    private event Action<WeaponAttack, int> OnValueChanged;

    public Item Item => _item;
    public Sprite PrimaryIcon => _primaryIcon;
    public Sprite SecondaryIcon => _secondaryIcon;
    public Sprite PassiveIcon => _passiveIcon;

    //Primary
    private readonly Timer primaryTimer = new();

    protected virtual float PrimaryCooldownDuration => 1;

    public virtual bool PrimaryAvailable => !primaryTimer.counting;
    public virtual float PrimaryCooldown => 1 - primaryTimer.percent;       //0 -> No cooldown, 1 -> Full cooldown

    public int PrimaryValue { get; private set; } = 0;
    public int PrimaryLevel { get; private set; } = 1;

    public virtual int PrimaryUpgradeCostBase => 10;
    public virtual int PrimaryUpgradeCostVariation => 10;
    public int PrimaryUpgradeCost => PrimaryUpgradeCostBase + (PrimaryLevel - 1) * PrimaryUpgradeCostVariation;

    //Secondary
    private readonly Timer secondaryTimer = new();

    protected virtual float SecondaryCooldownDuration => 1;

    public virtual bool SecondaryAvailable => !secondaryTimer.counting;
    public float SecondaryCooldown => 1 - secondaryTimer.percent;   //0 -> No cooldown, 1 -> Full cooldown

    public int SecondaryValue { get; private set; } = 0;
    public int SecondaryLevel { get; private set; } = 1;

    public virtual int SecondaryUpgradeCostBase => 10;
    public virtual int SecondaryUpgradeCostVariation => 10;
    public int SecondaryUpgradeCost => SecondaryUpgradeCostBase + (SecondaryLevel - 1) * SecondaryUpgradeCostVariation;

    //Passive
    private readonly Timer passiveTimer = new();

    protected virtual float PassiveCooldownDuration => 1;

    public virtual bool PassiveAvailable => !passiveTimer.counting;
    public virtual float PassiveCooldown => 1 - passiveTimer.percent;       //0 -> No cooldown, 1 -> Full cooldown

    public int PassiveValue { get; private set; } = 0;
    public int PassiveLevel { get; private set; } = 1;

    public virtual int PassiveUpgradeCostBase => 10;
    public virtual int PassiveUpgradeCostVariation => 10;
    public int PassiveUpgradeCost => PassiveUpgradeCostBase + (PassiveLevel - 1) * PassiveUpgradeCostVariation;


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

        //Visible -> Update weapon attack levels
        if (show) {
            PrimaryLevel = Loadout.GetUpgrade(GetUpgradeName(WeaponAttack.Primary));
            SecondaryLevel = Loadout.GetUpgrade(GetUpgradeName(WeaponAttack.Secondary));
            PassiveLevel = Loadout.GetUpgrade(GetUpgradeName(WeaponAttack.Passive));
        }

        //Init weapon
        Init();
    }

    protected virtual void Init() {}

    //Weapon
    protected void SetCooldown(WeaponAttack attack, float cooldown) {
        //Get timer
        Timer timer = attack switch {
            WeaponAttack.Primary => primaryTimer,
            WeaponAttack.Secondary => secondaryTimer,
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

    protected void SetValue(WeaponAttack attack, int value) {
        //Update value
        switch (attack) {
            case WeaponAttack.Primary:
                PrimaryValue = value;
                break;
            case WeaponAttack.Secondary:
                SecondaryValue = value;
                break;
            case WeaponAttack.Passive:
                PassiveValue = value;
                break;
        }

        //Call event
        OnValueChanged?.Invoke(attack, value);
    }

    public void AddOnValueChanged(Action<WeaponAttack, int> action) {
        OnValueChanged += action;
    }

    public void RemoveOnValueChanged(Action<WeaponAttack, int> action) {
        OnValueChanged -= action;
    }

    //Upgrades
    private string GetUpgradeName(WeaponAttack attack) {
        return $"{Item.FileName}_{attack}";
    }

    private int GetUpgradeCost(WeaponAttack attack) {
        return attack switch {
            WeaponAttack.Primary => PrimaryUpgradeCost,
            WeaponAttack.Secondary => SecondaryUpgradeCost,
            WeaponAttack.Passive => PassiveUpgradeCost,
            _ => 0
        };
    }

    public bool Upgrade(WeaponAttack attack) {
        //Get upgrade cost
        int cost = GetUpgradeCost(attack);

        //Pay for upgrade
        if (!Loadout.PayMoney(cost)) return false;

        //Upgrade
        string name = GetUpgradeName(attack);
        switch (attack) {
            case WeaponAttack.Primary:
                PrimaryLevel += 1;
                Loadout.SetUpgrade(name, PrimaryLevel);
                break;
            case WeaponAttack.Secondary:
                SecondaryLevel += 1;
                Loadout.SetUpgrade(name, SecondaryLevel);
                break;
            case WeaponAttack.Passive:
                PassiveLevel += 1;
                Loadout.SetUpgrade(name, PassiveLevel);
                break;
        }

        //Success
        return true;
    }

    //Primary
    protected virtual IEnumerator OnUsePrimaryCoroutine() { yield return null; }

    protected virtual void OnUsePrimary() {
        //Start cooldown
        SetCooldown(WeaponAttack.Primary, PrimaryCooldownDuration);

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
        SetCooldown(WeaponAttack.Secondary, SecondaryCooldownDuration);

        //Start use coroutine
        StartCoroutine(OnUseSecondaryCoroutine());
    }

    public bool UseSecondary() {
        if (!SecondaryAvailable) return false;
        OnUseSecondary();
        return true;
    }

    //Actions
    protected bool AtackForward(float damage, float radius, float forward) {
        //Casts a sphere of <radius> radius in front of the player and moves it forward <forward> amount to check for collisions
        var collisions = Physics.SphereCastAll(transform.position + radius * transform.forward, radius, transform.forward, forward);

        //Casts a sphere of <radius> radius in front of the player and moves it forward <forward> amount to check for collisions
        bool hit = false;

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

    protected bool AtackAround(float damage, float radius) {
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

    protected GameObject SpawnProjectile(GameObject prefab) {
        return Instantiate(prefab, transform.position, Player.transform.rotation);
    }

}