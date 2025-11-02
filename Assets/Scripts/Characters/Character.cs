using System;
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

    public bool IsAlive { get; protected set; } = true;
    public bool IgnoreNextDamage = false;
    public bool IsInvulnerable { get; protected set; } = false;
    public float Health { get; protected set; } = HEALTH_MAX;
    public virtual float HealthMax => HEALTH_MAX;

    public const float HEALTH_MAX = 100;


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
    public void AddOnHealthChanged(Action<float> action) {
        OnHealthChanged += action;
    }

    public void RemoveOnHealthChanged(Action<float> action) {
        OnHealthChanged -= action;
    }

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

        // Ignore this tick of damage
        if (IgnoreNextDamage)
        {
            IgnoreNextDamage = false;
            return false;
        }

        //Ignore negative damage
        if (amount <= 0) return false;

        //Damage character
        Health = Mathf.Max(Health - amount, 0);

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

    protected virtual void OnDeath(bool instant = false) {}

}
