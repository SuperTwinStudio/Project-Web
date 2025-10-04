using UnityEngine;

public class WorldItem : MonoBehaviour {

    //Item
    [Header("Item")]
    [SerializeField] private Item item;
    [SerializeField, Min(1)] private int amount = 1;


    //State
    private void OnTriggerEnter(Collider other) {
        //Check if player
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Player player)) return;

        //Add item to player
        player.Loadout.AddToInventory(item, amount);

        //Destroy self
        Destroy(gameObject);
    }

}
