using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Botpa;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable {

    //Character
    [Header("Character")]
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AttackHelper _attack;
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _eyes;
    [SerializeField] private Transform _bot;
    [SerializeField] private Transform _model;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Animator _animator;

    public AudioSource Audio => _audio;
    public AttackHelper Attack => _attack;
    public Transform Top => _top;
    public Transform Eyes => _eyes;
    public Transform Bot => _bot;
    public Transform Model => _model;
    public Renderer Renderer => _renderer;
    public Animator Animator => _animator;

    //Health
    [Header("Health")]
    [SerializeField] private float _baseHealth = DEFAULT_HEALTH_MAX;

    private event Action<float, float> OnHealthChanged;
    private Coroutine damageFeedbackCoroutine = null;

    public bool IgnoreNextDamage { get; set; } = false;
    public bool IsInvulnerable { get; set; } = false;

    public bool IsAlive { get; protected set; } = true;
    public float Health { get; protected set; } = DEFAULT_HEALTH_MAX;
    public virtual float HealthMax => _baseHealth + EffectExtraHealth;

    public const float DEFAULT_HEALTH_MAX = 100;

    //Effects
    [Header("Effects")]
    [SerializeField] private ParticleSystem burn;

    protected readonly Dictionary<Effect, (float, int)> effects = new(); //Stores effects & its duration & level

    public float EffectSlowSpeedMultiplier { get; private set; } = 1;
    public float EffectFastSpeedMultiplier { get; private set; } = 1;
    public float EffectDamageTakenMultiplier { get; private set; } = 1;
    public float EffectDamageDealtMultiplier { get; private set; } = 1;
    public float EffectExtraHealth { get; private set; } = 0;

    public float SpeedMultiplier => EffectSlowSpeedMultiplier * EffectFastSpeedMultiplier;

    //Movement & Rotation
    public virtual Vector3 MoveVelocity => Vector3.zero;


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

    //Movement & Rotation
    public virtual void TeleportTo(Vector3 position) {
        transform.position = position;
    }

    public virtual void Push(Vector3 direction) {
        Debug.Log("Push not implemented");
    }

    public virtual void LookTowards(Vector3 position) {
        //Update rotation
        Model.rotation = Quaternion.LookRotation(Util.RemoveY(position - Model.position).normalized);
    }

    //Health
    private IEnumerator DamageFeedbackCoroutine(DamageType type) {
        OnDamageFeedbackStart(type);
        yield return new WaitForSeconds(0.08f);
        OnDamageFeedbackEnd(type);
    }

    protected virtual void OnDamageFeedbackStart(DamageType type) {
        //Slow time
        Game.SlowTime(this);
    }

    protected virtual void OnDamageFeedbackEnd(DamageType type) {
        //Stop slowing time
        Game.UnslowTime(this);
    }

    protected virtual void OnDamageFeedbackCancel() {
        //Stop slowing time
        Game.UnslowTime(this);
    }

    protected virtual void OnDeath() {
        //Stop burn particles
        burn.Stop();
    }

    public virtual bool Revive(float health) {
        //Character is already alive
        if (IsAlive) return false;

        //Heal character
        Health = Mathf.Min(health, HealthMax);
        IsAlive = true;

        //Call event
        CallOnHealthChanged();

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
        CallOnHealthChanged();

        //Healed
        return true;
    }

    public virtual bool Damage(float amount, DamageType type, object source) {
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
            if (damageFeedbackCoroutine != null) {
                Game.Current.StopCoroutine(damageFeedbackCoroutine);
                OnDamageFeedbackCancel();
            }
            damageFeedbackCoroutine = Game.Current.StartCoroutine(DamageFeedbackCoroutine(type));
        }

        //Call event
        CallOnHealthChanged();

        //Damaged
        return true;
    }

    protected void CallOnHealthChanged() {
        OnHealthChanged?.Invoke(Health, HealthMax);
    }

    public void AddOnHealthChanged(Action<float, float> action) {
        OnHealthChanged += action;
    }

    public void RemoveOnHealthChanged(Action<float, float> action) {
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
                    Damage(Time.deltaTime * value, DamageType.Burn, this); //Take value as damage per second
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

            //Disable burn particles
            if (effect.FileName == "Burn") burn.Stop();

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
        //Dead -> Ignore
        if (!IsAlive) return;

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

        //Enable burn particles
        if (effect.FileName == "Burn") burn.Play();

        //Apply instant application effects
        switch (effect.Action.Type) {
            //Max health
            case EffectType.MaxHealth:
                EffectExtraHealth += effect.Action.Value;
                break;
        }
    }

    public void RemoveEffect(Effect effect) {
        //Check if player has effect
        if (!effects.ContainsKey(effect)) return;

        //Get effect
        (float currentEndTimestamp, int currentLevel) = effects[effect];

        //Check if effect has levels
        if (effect.HasLevels) {
            //Has levels -> Remove a level
            int newLevel = currentLevel - 1;

            //Check new level
            if (newLevel >= 1) {
                //Still has levels -> Update it
                effects[effect] = (currentEndTimestamp, newLevel);
            } else {
                //No levels -> Remove it
                effects.Remove(effect);
            }
        } else {
            //No levels -> Remove it
            effects.Remove(effect);
        }

        //Unapply instant application effects
        switch (effect.Action.Type) {
            //Max health
            case EffectType.MaxHealth:
                EffectExtraHealth -= effect.Action.Value;
                break;
        }
    }

    //Attack
    public virtual float CalculateDamage(float damage) {
        return damage * EffectDamageDealtMultiplier;
    }

    //Sound
    public void PlaySound(AudioClip clip) {
        Audio.pitch = UnityEngine.Random.Range(0.92f, 1.08f);
        Audio.PlayOneShot(clip);
    }

}
