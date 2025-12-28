using System;
using Botpa;
using UnityEngine;

public class AttackHelper : MonoBehaviour {

    //Components
    private Loadout Loadout => Game.Current.Level.Player.Loadout;

    //Opions
    [Header("Opions")]
    [SerializeField] private bool _isPlayer = true;
    [SerializeField] private Character _source;
    [SerializeField] private GameObject _AOEIndicator;
    [SerializeField] private Transform _model;

    public bool IsPlayer => _isPlayer;
    public Character Source => _source;
    public GameObject AOEIndicator => _AOEIndicator;
    public Transform Model => _model;


    //Melee
    private bool DamageHits(RaycastHit[] hits, float damage, Action<IDamageable> onHit = null) {
        //Calculate damage
        damage = Source.CalculateDamage(damage);

        //Bool to check if anything was hit
        bool somethingHit = false;

        //Check hits
        foreach (var hit in hits) {
            //Check if collision is a damageable
            if (!hit.collider.TryGetComponent(out IDamageable damageable)) continue;

            //Ignore player
            if (IsPlayer ? damageable is Player : damageable is not Player) continue;

            //Damage
            if (damage > 0) {
                //Damage
                damageable.Damage(damage, DamageType.Melee, Source);

                //Tell loadout a damageable was hit
                if (IsPlayer) Loadout.OnDamageableHit(hit.collider.gameObject);
            }

            //Mark as hit
            onHit?.Invoke(damageable);
            somethingHit = true;
        }

        //Return if anything was hit
        return somethingHit;
    }

    protected RaycastHit[] AttackForwardCheck(bool showAOE, float radius, float forward) {
        //Get forward direction
        Vector3 forwardDirection = Model.forward;

        //Show AOE indicator
        if (showAOE) ShowAOE(radius, forward, true);

        //Casts a sphere of <radius> radius in front of the player and moves it forward <forward> amount to check for collisions
        return Physics.SphereCastAll(Model.position + radius * forwardDirection, radius, forwardDirection, forward);
    }

    protected RaycastHit[] AttackAroundCheck(bool showAOE, float radius) {
        //Show AOE indicator
        if (showAOE) ShowAOE(radius, 0, false);

        //Casts a sphere of <radius> radius around the player
        return Physics.SphereCastAll(Model.position, radius, Vector3.up, 0);
    }

    public bool Forward(float radius, float forward, float damage, bool showAOE = true, Action<IDamageable> onHit = null) {
        return DamageHits(AttackForwardCheck(showAOE, radius, forward), damage, onHit);
    }

    public bool Around(float radius, float damage, bool showAOE = true, Action<IDamageable> onHit = null) {
        return DamageHits(AttackAroundCheck(showAOE, radius), damage, onHit);
    }

    public void ShowAOE(float radius, float forward, bool inFront, float duration = -1) {
        //No prefab or disabled in settings -> Return
        if (!AOEIndicator || !Preferences.ShowAOE) return;

        //Create indicator
        AttackAreaIndicator indicator = Instantiate(AOEIndicator, Model.position + 0.1f * Vector3.up, Model.rotation).GetComponent<AttackAreaIndicator>();
        indicator.GenerateIndicator(radius, forward, inFront);
        if (duration > 0) indicator.Disappear(duration);
    }

    //Ranged
    public Projectile Throw(GameObject prefab, float damage, Transform origin, Vector3 direction = new()) {
        Projectile projectile = Instantiate(prefab, origin.position, Quaternion.LookRotation(direction.IsEmpty() ? origin.forward : direction)).GetComponent<Projectile>();
        projectile.Init(Source, Source.CalculateDamage(damage));
        return projectile;
    }

}
