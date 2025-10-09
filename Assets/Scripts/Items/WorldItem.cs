using UnityEngine;

public class WorldItem : MonoBehaviour {

    //Item
    [Header("Item")]
    [SerializeField] private Item item;
    [SerializeField, Min(1)] private int amount = 1;

    public void SetItem(Item item)
    {
        this.item = item;
    }

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
