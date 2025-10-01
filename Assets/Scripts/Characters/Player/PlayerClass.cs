using Botpa;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerClassCategory { Melee, Ranged, Effect }

public class PlayerClass : MonoBehaviour {

    //Item
    [Header("Item")]
    [SerializeField] private Item _item;

    public Item Item => _item;

    //Primary
    [Header("Primary")]
    [SerializeField] private PlayerClassCategory primaryCategory = PlayerClassCategory.Melee;
    [SerializeField, Min(0)] private float primaryCooldown = 1;
    [SerializeField, Min(0)] private float primaryRange = 1; //For melee by now
    [SerializeField] private TurboEventGroup primaryOnUse = new();

    //Primary
    /*
    [Header("Primary")]
    [SerializeField] private PlayerClassCategory secondaryCategory = PlayerClassCategory.Effect;
    [SerializeField] private float secodaryRange = 1;
    */

    //Use
    public void UsePrimary() {

    }

    //Actions
    public void SpawnProjectile(GameObject prefab) {

    }

}
