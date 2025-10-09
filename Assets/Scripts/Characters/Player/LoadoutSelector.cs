using System.Linq;
using UnityEngine;

public class LoadoutSelector : MonoBehaviour {

    //Weapon
    [Header("Weapon")]
    [SerializeField] private Item weapon;


    //State
    private void Start() {
        ToggleActive();
    }

    public void ToggleActive() {
        //Show if
        gameObject.SetActive(!weapon || Game.Current.Level.Player.Loadout.Unlocked.Contains(weapon));
    }

    private void OnTriggerEnter(Collider other) {
        //Check if other is player
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Player player)) return;

        //Unlock weapon
        if (weapon) {
            //Has weapon -> Select it
            player.Loadout.SelectWeapon(weapon);
        } else {
            //No weapon -> Unlock all
            player.Loadout.UnlockAllWeapons();
            foreach (var selector in FindObjectsByType<LoadoutSelector>(FindObjectsInactive.Include, FindObjectsSortMode.None)) selector.ToggleActive();
        }
    }

}
