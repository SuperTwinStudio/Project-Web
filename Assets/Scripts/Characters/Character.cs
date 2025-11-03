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

    public bool IgnoreNextDamage { get; protected set; } = false;
    public bool IsInvulnerable { get; protected set; } = false;

    public bool IsAlive { get; protected set; } = true;
    public float Health { get; protected set; } = HEALTH_MAX;
    public virtual float HealthMax => HEALTH_MAX;

    public const float HEALTH_MAX = 100;

    //Effects
    protected readonly Dictionary<Effect, float> effects = new();
    protected float slowSpeedMultiplier = 1;


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
            if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, viewDistance, layers)) {
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

    protected virtual void OnDeath(bool instant = false) {}

    public virtual bool Heal(float amount) {
        //Ignore negative healing
        if (amount <= 0) return false;

        //Heal character
        Health = Mathf.Min(Health + amount, HealthMax);

        //Call event
        OnHealthChanged?.Invoke(Health);

        //Healed
        return true;
    }

    public virtual bool Damage(float amount, object source) {
        //Character is already dead or invulnerable-> Ignore damage
        if (!IsAlive || IsInvulnerable) return false;

        //Ignore this tick of damage
        if (IgnoreNextDamage) {
            IgnoreNextDamage = false;
            return false;
        }

        //Ignore negative damage
        if (amount <= 0) return false;

        //Damage character
        Health = Mathf.Max(Health - amount, 0);

        //Start damage feedback coroutine
        if (damageFeedbackCoroutine != null) StopCoroutine(damageFeedbackCoroutine);
        damageFeedbackCoroutine = Game.Current.StartCoroutine(DamageFeedbackCoroutine()); //Start coroutine in game cause it will never get destroyed

        //Check if character died
        if (Health <= 0) {
            //Character died -> Call OnDeath
            IsAlive = false;
            OnDeath();
        }

        //Call event
        OnHealthChanged?.Invoke(Health);

        //Damaged
        return true;
    }

    public void AddOnHealthChanged(Action<float> action) {
        OnHealthChanged += action;
    }

    public void RemoveOnHealthChanged(Action<float> action) {
        OnHealthChanged -= action;
    }

    //Effects
    protected void UpdateEffects() {
        //Get current time
        float nowTimestamp = Time.time;

        //Reset slow multiplier
        slowSpeedMultiplier = 1;

        //Apply effects
        foreach (var effect in effects.Keys.ToList()) {
            //Get end timestamp
            float endTimestamp = effects[effect];

            //Apply effect
            switch (effect.Action.Type) {
                //Damage
                case EffectType.Damage:
                    Damage(Time.deltaTime * effect.Action.Points, this);  //Take points as damage per second
                    break;
                //Heal
                case EffectType.Heal:
                    Heal(Time.deltaTime * effect.Action.Points);    //Take points as healing per second
                    break;
                //Slow
                case EffectType.Slow:
                    slowSpeedMultiplier = Mathf.Min(slowSpeedMultiplier, Mathf.Clamp01(1 - effect.Action.Points));
                    break;
            }

            //Check if effect finished
            if (nowTimestamp > endTimestamp) effects.Remove(effect);
        }
    }

    public void AddEffect(Effect effect, float duration) {
        //Calculate effect end timestamp
        float effectEndTimestamp = Time.time + duration;

        //Check if player already has effect
        if (effects.ContainsKey(effect)) {
            //Already has effect -> Check to update duration
            effects[effect] = Mathf.Max(effects[effect], effectEndTimestamp);
        } else {
            //Does not have effect -> Add it
            effects[effect] = effectEndTimestamp;
        }
    }

}
