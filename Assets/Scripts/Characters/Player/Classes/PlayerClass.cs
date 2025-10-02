using System.Collections;
using UnityEngine;

public class PlayerClass : MonoBehaviour {

    //Components
    protected Player Player => Game.Current.Level.Player;

    //Class
    [Header("Class")]
    [SerializeField] private Item _item;
    [SerializeField] private GameObject model;
    [SerializeField] protected Animator animator;

    public Item Item => _item;

    //Attacks
    public bool CanUsePrimary { get; protected set; } = true;
    public bool CanUseSecondary { get; protected set; } = true;


    //Attacks
    protected virtual IEnumerator PrimaryAttack() {
        //Wait
        yield return null;

        //Finish attack
        CanUsePrimary = true;
    }
    
    protected virtual IEnumerator SecondaryAttack() {
        //Wait
        yield return null;

        //Finish attack
        CanUseSecondary = true;
    }

    public void UsePrimary() {
        //Can't use
        if (!CanUsePrimary) return;

        //Start attack
        CanUsePrimary = false;
        StartCoroutine(PrimaryAttack());
    }

    public void UseSecondary() {
        //Can't use
        if (!CanUseSecondary) return;

        //Start attack
        CanUseSecondary = false;
        StartCoroutine(SecondaryAttack());
    }

    //Visibility
    public void Show(bool show) {
        model.SetActive(show);
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
