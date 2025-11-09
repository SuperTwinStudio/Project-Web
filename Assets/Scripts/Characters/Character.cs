using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable {

    //Character
    [Header("Character")]
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _eyes;
    [SerializeField] private Transform _bot;

    public Transform Top => _top;
    public Transform Eyes => _eyes;
    public Transform Bot => _bot;

    //Health
    private event Action<float> OnHealthChanged;
    private Coroutine damageFeedbackCoroutine = null;

    public bool IgnoreNextDamage { get; set; } = false;
    public bool IsInvulnerable { get; set; } = false;

    public bool IsAlive { get; set; } = true;
    public float Health { get; protected set; } = DEFAULT_HEALTH_MAX;
    public virtual float HealthMax => DEFAULT_HEALTH_MAX + EffectExtraHealth;

    public const float DEFAULT_HEALTH_MAX = 100;

    //Effects
    protected readonly Dictionary<Effect, (float, int)> effects = new(); //Stores effects & its duration & level

    public float EffectSlowSpeedMultiplier { get; private set; } = 1;
    public float EffectFastSpeedMultiplier { get; private set; } = 1;
    public float EffectDamageTakenMultiplier { get; private set; } = 1;
    public float EffectDamageDealtMultiplier { get; private set; } = 1;
    public float EffectExtraHealth { get; private set; } = 0;

    public float SpeedMultiplier => EffectSlowSpeedMultiplier * EffectFastSpeedMultiplier;


    //State
    private void Update() {
        //Game is paused | Character is dead | A menu is transitioning
        if (Game.IsPaused || !IsAlive || Game.Current.MenuManager.InTransition) return;

        //Update effects
        UpdateEffects();

        //Character is dead
        if (!IsAlive) return;

        //Update character
        OnUpdate();
    }

    protected virtual void OnUpdate() {}

    //Visibility
    public bool IsVisible(Vector3 origin, float viewDistance, LayerMask layers) {
        //Get bot & top points
        Vector3 top = Top.position;
        Vector3 bot = Bot.position;

        //Get top to bot direction vector (visibility chech goes from head to feet)
        Vector3 topToBot = bot - top;

        //Check if visible
        int maxChecks = 10;
        for (int i = 0; i < maxChecks; i++) {
            //Get direction
            float percent = (float)i / (maxChecks - 1);
            Vector3 direction = top + topToBot * percent - origin;

            //Raycast
            if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, viewDistance, layers, QueryTriggerInteraction.Ignore)) {
                if (hit.collider.CompareTag("Player")) {
                    //Debug hit
                    Debug.DrawRay(origin, direction.normalized * hit.distance, Color.green);

                    //Player is visible
                    return true;
                }
                //Debug miss
                Debug.DrawRay(origin, direction.normalized * hit.distance, new Color(1, 0.65f, 0));
            } else {
                //Debug miss
                Debug.DrawRay(origin, direction.normalized * viewDistance, Color.red);
            }
        }

        //Not visible
        return false;
    }

    //Movement
    public virtual void TeleportTo(Vector3 position) {
        transform.position = position;
    }

    public virtual void Push(Vector3 direction) {
        Debug.Log("Push not implemented");
    }

    //Health
    private IEnumerator DamageFeedbackCoroutine() {
        OnDamageFeedbackStart();
        yield return new WaitForSeconds(0.08f);
        OnDamageFeedbackEnd();
    }

    protected virtual void OnDamageFeedbackStart() {
        Game.SlowTime(this);
    }

    protected virtual void OnDamageFeedbackEnd() {
        Game.UnslowTime(this);
    }

    public virtual bool Revive(float health) {
        //Character is already alive
        if (IsAlive) return false;

        //Heal character
        Health = Mathf.Min(health, HealthMax);
        IsAlive = true;

        //Call event
        OnHealthChanged?.Invoke(Health);

        //Revived
        return true;
    }

    public virtual bool Heal(float amount) {
        //Character is dead -> Ignore healing
        if (!IsAlive) return false;

        //Ignore negative healing
        if (amount <= 0) return false;

        //Heal character
        Health = Mathf.Min(Health + amount, HealthMax);

        //Call event
        OnHealthChanged?.Invoke(Health);

        //Healed
        return true;
    }

    public virtual bool Damage(float amount, object source, DamageType type = DamageType.None) {
        //Character is already dead or invulnerable -> Ignore damage
        if (!IsAlive || IsInvulnerable) return false;

        //Ignore this tick of damage
        if (IgnoreNextDamage) {
            IgnoreNextDamage = false;
            return false;
        }

        //Ignore negative damage
        if (amount <= 0) return false;

        float damage = amount * EffectDamageTakenMultiplier;

        //Damage character
        Health = Mathf.Max(Health - damage, 0);

        //Check if character died
        if (Health <= 0) {
            //Character died -> Call OnDeath
            IsAlive = false;
            OnDeath();
        }

        //Start damage feedback coroutine (if object was not destroyed)
        if (this) {
            //Use game for coroutine cause it will never get destroyed
            if (damageFeedbackCoroutine != null) Game.Current.StopCoroutine(damageFeedbackCoroutine);
            damageFeedbackCoroutine = Game.Current.StartCoroutine(DamageFeedbackCoroutine());
        }

        //Call event
        OnHealthChanged?.Invoke(Health);

        //Damaged
        return true;
    }

    protected virtual void OnDeath() {}

    public void AddOnHealthChanged(Action<float> action) {
        OnHealthChanged += action;
    }

    public void RemoveOnHealthChanged(Action<float> action) {
        OnHealthChanged -= action;
    }

    //Effects
    protected virtual void OnEffectsUpdated() {}

    protected void UpdateEffects() {
        //Get current time
        float nowTimestamp = Time.time;

        //Reset effects
        EffectSlowSpeedMultiplier = 1;
        EffectFastSpeedMultiplier = 1;
        EffectDamageTakenMultiplier = 1;
        EffectDamageDealtMultiplier = 1;

        //Apply effects
        foreach (var effect in effects.Keys.ToList()) {
            //Get end timestamp
            (float endTimestamp, int level) = effects[effect];
            float value = effect.Action.Value * level;

            //Apply effect
            switch (effect.Action.Type) {
                //Damage
                case EffectType.Damage:
                    Damage(Time.deltaTime * value, this, DamageType.Burn); //Take value as damage per second
                    break;
                //Heal
                case EffectType.Heal:
                    Heal(Time.deltaTime * value); //Take value as healing per second
                    break;
                //Slowness (slow movement)
                case EffectType.Slowness:
                    EffectSlowSpeedMultiplier = Mathf.Min(EffectSlowSpeedMultiplier, Mathf.Clamp01(1 - value)); //Take value as slow percentaje
                    break;
                //Fastness (fast movement)
                case EffectType.Fastness:
                    EffectFastSpeedMultiplier += Mathf.Max(0, value); //Take value as speed percentaje
                    break;
                //Weakness (take more damage)
                case EffectType.Weakness:
                    EffectDamageTakenMultiplier = Mathf.Max(EffectDamageTakenMultiplier, 1 + value); //Take value as damage taken percentaje
                    break;
                //Strength (deal more damage)
                case EffectType.Strength:
                    EffectDamageDealtMultiplier = Mathf.Max(EffectDamageDealtMultiplier, 1 + value); //Take value as damage dealt percentaje
                    break;
            }

            //Check if effect hasn't finished
            if (nowTimestamp < endTimestamp) continue;

            //Remove effect
            effects.Remove(effect);

            //Unapply instant application effects
            switch (effect.Action.Type) {
                //Max health
                case EffectType.MaxHealth:
                    EffectExtraHealth -= effect.Action.Value;
                    break;
            }
        }

        //Call on effects updated
        OnEffectsUpdated();
    }

    public bool TryGetEffect(Effect effect, out float endTimestamp, out int level) {
        if (effects.ContainsKey(effect)) {
            //Has effect
            (endTimestamp, level) = effects[effect];
            return true;
        } else {
            //Does not have effect
            (endTimestamp, level) = (0, 0);
            return true;
        }
    }

    public void AddEffect(Effect effect, float duration = float.PositiveInfinity) {
        //Calculate effect end timestamp
        float effectEndTimestamp = Time.time + duration;

        //Check if player already has effect
        if (effects.ContainsKey(effect)) {
            //Already has effect -> Update it
            (float currentEndTimestamp, int currentLevel) = effects[effect];
            effects[effect] = (
                Mathf.Max(currentEndTimestamp, effectEndTimestamp),                             //End timestamp
                effect.HasLevels ? Mathf.Min(currentLevel + 1, effect.MaxLevel) : currentLevel  //Level
            );
        } else {
            //Does not have effect -> Add it
            effects[effect] = (
                effectEndTimestamp, //End timestamp
                1                   //Level
            );
        }

        //Apply instant application effects
        switch (effect.Action.Type) {
            //Max health
            case EffectType.MaxHealth:
                EffectExtraHealth += effect.Action.Value;
                break;
        }
    }

}
