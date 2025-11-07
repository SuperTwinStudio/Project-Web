using System;
using System.Collections;
using Botpa;
using UnityEngine;
using UnityEngine.Localization;

public enum WeaponAction { Primary, Secondary, Passive, Reload }

public class Weapon : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] protected Loadout _loadout;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected RuntimeAnimatorController animatorController;

    protected Loadout Loadout => _loadout;
    protected Player Player => Loadout.Player;
    protected virtual Animator Animator => Player.Animator;
    protected CameraController CameraController => Player.CameraController;

    //Weapon
    [Header("Weapon")]
    [SerializeField] private Item _item;
    [SerializeField] private Sprite _primaryIcon;
    [SerializeField] private LocalizedString _primaryDescription;
    [SerializeField] private Sprite _secondaryIcon;
    [SerializeField] private LocalizedString _secondaryDescription;
    [SerializeField] private Sprite _passiveIcon;
    [SerializeField] private LocalizedString _passiveDescription;
    [SerializeField] private GameObject model;

    private event Action<WeaponAction, int> OnValueChanged;

    private bool isInit = false;

    public Item Item => _item;
    public Sprite PrimaryIcon => _primaryIcon;
    public string PrimaryDescription => _primaryDescription.GetLocalizedString();
    public Sprite SecondaryIcon => _secondaryIcon;
    public string SecondaryDescription => _secondaryDescription.GetLocalizedString();
    public Sprite PassiveIcon => _passiveIcon;
    public string PassiveDescription => _passiveDescription.GetLocalizedString();

    [HideInInspector] public GameObject LastHit { get; protected set; }

    //Primary
    private readonly Timer primaryTimer = new();

    protected virtual float PrimaryCooldownDuration => 1;

    public virtual bool PrimaryAvailable => !primaryTimer.counting;
    public virtual float PrimaryCooldown => 1 - primaryTimer.percent; //0 -> No cooldown, 1 -> Full cooldown

    public Upgrade PrimaryUpgrade { get; protected set; }

    public int PrimaryValue { get; private set; } = 0;

    //Secondary
    private readonly Timer secondaryTimer = new();

    protected virtual float SecondaryCooldownDuration => 1;

    public virtual bool SecondaryAvailable => !secondaryTimer.counting;
    public float SecondaryCooldown => 1 - secondaryTimer.percent; //0 -> No cooldown, 1 -> Full cooldown

    public Upgrade SecondaryUpgrade { get; protected set; }

    public int SecondaryValue { get; private set; } = 0;

    //Passive
    private readonly Timer passiveTimer = new();

    protected virtual float PassiveCooldownDuration => 1;

    public virtual bool PassiveAvailable => !passiveTimer.counting;
    public virtual float PassiveCooldown => 1 - passiveTimer.percent; //0 -> No cooldown, 1 -> Full cooldown

    public Upgrade PassiveUpgrade { get; protected set; }

    public int PassiveValue { get; private set; } = 0;

    //Reload
    public int ReloadValue { get; private set; } = -1;

    public virtual bool IsReloading => ReloadValue >= 0;


    //State
    private void Init() {
        //Check if already init
        if (isInit) return;
        isInit = true;

        //Reset timers
        primaryTimer.Count(0);
        secondaryTimer.Count(0);
        passiveTimer.Count(0);

        //Init upgrades
        string name;
        name = GetUpgradeName(WeaponAction.Primary);
        PrimaryUpgrade = new(name, Loadout.GetUpgrade(name), Upgrade.DEFAULT_LEVEL_MAX);
        name = GetUpgradeName(WeaponAction.Secondary);
        SecondaryUpgrade = new(name, Loadout.GetUpgrade(name), Upgrade.DEFAULT_LEVEL_MAX);
        name = GetUpgradeName(WeaponAction.Passive);
        PassiveUpgrade = new(name, Loadout.GetUpgrade(name), Upgrade.DEFAULT_LEVEL_MAX);

        //Weapon custom on init
        OnInit();
    }

    private void Awake() {
        //Init
        Init();
    }

    protected virtual void OnInit() {}

    public void Show(bool show) {
        //Init
        Init();

        //Visible -> Refresh upgrade levels
        if (show) RefreshUpgradeLevels();

        //Toggle model
        model.SetActive(show);

        //Change player animator controller (remove the if when all controllers are done)
        if (show && animatorController) Animator.runtimeAnimatorController = animatorController;

        //Weapon custom on show
        OnShow();
    }

    protected virtual void OnShow() {}

    //Audio
    protected void PlaySound(AudioClip clip) {
        audioSource.pitch = UnityEngine.Random.Range(0.92f, 1.08f);
        audioSource.PlayOneShot(clip);
        //audioSource.clip = clip;
        //audioSource.Play();
    }

    //Weapon
    protected void SetCooldown(WeaponAction attack, float cooldown) {
        //Get timer
        Timer timer = attack switch {
            WeaponAction.Primary => primaryTimer,
            WeaponAction.Secondary => secondaryTimer,
            _ => passiveTimer,
        };

        //Check if timer needs to count or extend
        if (timer.counting) {
            //Already counting -> Check if count is needed
            float remaining = timer.duration - timer.counted;
            if (remaining < cooldown) timer.Count(cooldown);//timer.Extend(cooldown - remaining);
        } else {
            //Not counting -> Count
            timer.Count(cooldown);
        }
    }

    protected void SetValue(WeaponAction attack, int value) {
        //Update value
        switch (attack) {
            case WeaponAction.Primary:
                PrimaryValue = value;
                break;
            case WeaponAction.Secondary:
                SecondaryValue = value;
                break;
            case WeaponAction.Passive:
                PassiveValue = value;
                break;
            case WeaponAction.Reload:
                ReloadValue = value;
                break;
        }

        //Call event
        OnValueChanged?.Invoke(attack, value);
    }

    public void AddOnValueChanged(Action<WeaponAction, int> action) {
        OnValueChanged += action;
    }

    public void RemoveOnValueChanged(Action<WeaponAction, int> action) {
        OnValueChanged -= action;
    }

    public virtual float GetWeaponBaseDamage() { throw new NotImplementedException(); }

    //Upgrades
    private void RefreshUpgradeLevels() {
        PrimaryUpgrade.SetLevel(Loadout.GetUpgrade(PrimaryUpgrade.Key));
        SecondaryUpgrade.SetLevel(Loadout.GetUpgrade(SecondaryUpgrade.Key));
        PassiveUpgrade.SetLevel(Loadout.GetUpgrade(PassiveUpgrade.Key));
    }

    protected string GetUpgradeName(WeaponAction attack) {
        return $"{Item.FileName}_{attack}";
    }

    public Upgrade GetUpgrade(WeaponAction attack) {
        return attack switch {
            WeaponAction.Primary => PrimaryUpgrade,
            WeaponAction.Secondary => SecondaryUpgrade,
            _ => PassiveUpgrade
        };
    }

    public bool TryUpgrade(WeaponAction attack) {
        return GetUpgrade(attack).TryUpgrade(Loadout);
    }

    //Primary
    protected virtual IEnumerator OnUsePrimaryCoroutine() { yield return null; }

    protected virtual void OnUsePrimary() {
        //Start cooldown
        SetCooldown(WeaponAction.Primary, PrimaryCooldownDuration);

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
        SetCooldown(WeaponAction.Secondary, SecondaryCooldownDuration);

        //Start use coroutine
        StartCoroutine(OnUseSecondaryCoroutine());
    }

    public bool UseSecondary() {
        if (!SecondaryAvailable) return false;
        OnUseSecondary();
        return true;
    }

    //Reload
    protected virtual bool OnReload() {
        return false;
    }

    public bool Reload() {
        return OnReload();
    }

    //Actions
    private bool DamageHits(RaycastHit[] hits, float damage, Action<IDamageable> onHit = null) {
        //Bool to check if anything was hit
        bool somethingHit = false;

        //Check hits
        foreach (var hit in hits) {
            //Check if collision is a damageable
            if (!hit.collider.TryGetComponent(out IDamageable damageable)) continue;

            //Ignore player
            if (damageable is Player) continue;

            //Damage
            if (damage > 0) {
                Loadout.OnDamageableHit(hit.collider.gameObject);
                damageable.Damage(damage * Player.DamageMultiplier, Player, DamageType.Melee);
            }

            //Mark as hit
            onHit?.Invoke(damageable);
            somethingHit = true;
        }

        //Return if anything was hit
        return somethingHit;
    }

    protected RaycastHit[] MeleeForwardCheck(float radius, float forward) {
        //Get forward direction
        Vector3 forwardDirection = transform.forward;

        //Casts a sphere of <radius> radius in front of the player and moves it forward <forward> amount to check for collisions
        return Physics.SphereCastAll(transform.position + radius * forwardDirection, radius, forwardDirection, forward);
    }

    protected RaycastHit[] MeleeAroundCheck(float radius) {
        //Casts a sphere of <radius> radius around the player
        return Physics.SphereCastAll(transform.position, radius, Vector3.up, 0);
    }

    protected bool MeleeForward(float radius, float forward, float damage, Action<IDamageable> onHit = null) {
        return DamageHits(MeleeForwardCheck(radius, forward), damage, onHit);
    }

    protected bool MeleeAround(float radius, float damage, Action<IDamageable> onHit = null) {
        return DamageHits(MeleeAroundCheck(radius), damage, onHit);
    }

    protected GameObject SpawnProjectile(GameObject prefab, Transform origin = null) {
        return Instantiate(prefab, (origin ? origin : transform).position, Player.transform.rotation);
    }

}