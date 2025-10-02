using System;
using System.Collections;
using Botpa;
using UnityEngine;

public enum ClassType { Primary, Secondary, Passive }

public class PlayerClass : MonoBehaviour {

    //Components
    protected Player Player => Game.Current.Level.Player;

    //Info
    [Header("Info")]
    [SerializeField] private Item _item;
    [SerializeField] private GameObject model;
    [SerializeField] protected Animator animator;

    private event Action<ClassType, int> OnValueChanged;

    public Item Item => _item;

    //Primary
    private readonly Timer primaryTimer = new();

    protected virtual float PrimaryCooldownDuration => 1;

    public virtual bool PrimaryAvailable => !primaryTimer.counting;
    public virtual float PrimaryCooldown => 1 - primaryTimer.percent;       //0 -> No cooldown, 1 -> Full cooldown

    public int PrimaryValue { get; private set; } = 0;

    //Secondary
    private readonly Timer secondaryTimer = new();

    protected virtual float SecondaryCooldownDuration => 1;

    public virtual bool SecondaryAvailable => !secondaryTimer.counting;
    public virtual float SecondaryCooldown => 1 - secondaryTimer.percent;   //0 -> No cooldown, 1 -> Full cooldown

    public int SecondaryValue { get; private set; } = 0;

    //Passive
    private readonly Timer passiveTimer = new();

    protected virtual float PassiveCooldownDuration => 1;

    public virtual bool PassiveAvailable => !passiveTimer.counting;
    public virtual float PassiveCooldown => 1 - passiveTimer.percent;       //0 -> No cooldown, 1 -> Full cooldown

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
    }

    //Class
    protected void SetCooldown(ClassType type, float cooldown) {
        //Get timer
        Timer timer = type switch {
            ClassType.Primary => primaryTimer,
            ClassType.Secondary => secondaryTimer,
            _ => passiveTimer,
        };

        //Check if timer needs to count or extend
        if (timer.counting) {
            //Already counting -> Check if count is needed
            float remaining = timer.duration - timer.counted;
            if (remaining < cooldown) timer.Count(cooldown);
        } else {
            //Not counting -> Count
            timer.Count(cooldown);
        }
    }

    protected void SetValue(ClassType type, int value) {
        //Update value
        switch (type) {
            case ClassType.Primary:
                PrimaryValue = value;
                break;
            case ClassType.Secondary:
                SecondaryValue = value;
                break;
            case ClassType.Passive:
                PassiveValue = value;
                break;
        }

        //Call event
        OnValueChanged?.Invoke(type, value);
    }

    public void AddOnValueChanged(Action<ClassType, int> action) {
        OnValueChanged += action;
    }

    public void RemoveOnValueChanged(Action<ClassType, int> action) {
        OnValueChanged -= action;
    }

    //Primary
    protected virtual IEnumerator OnUsePrimaryCoroutine() { yield return null; }

    protected virtual void OnUsePrimary() {
        //Start cooldown
        SetCooldown(ClassType.Primary, PrimaryCooldownDuration);

        //Start use coroutine
        StartCoroutine(OnUsePrimaryCoroutine());
    }

    public void UsePrimary() {
        if (PrimaryAvailable) OnUsePrimary();
    }

    //Secondary
    protected virtual IEnumerator OnUseSecondaryCoroutine() { yield return null; }

    protected virtual void OnUseSecondary() {
        //Start cooldown timer
        SetCooldown(ClassType.Secondary, SecondaryCooldownDuration);

        //Start use coroutine
        StartCoroutine(OnUseSecondaryCoroutine());
    }

    public void UseSecondary() {
        if (SecondaryAvailable) OnUseSecondary();
    }

    //Actions
    public bool AtackForward(float damage, float radius, float distance) {
        //Casts a sphere of <radius> radius in front of the player and moves it forward <distance> amount to check for collisions
        bool hit = false;

        //Cast attack
        var collisions = Physics.SphereCastAll(transform.position + radius * transform.forward, radius, transform.forward, distance);

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
