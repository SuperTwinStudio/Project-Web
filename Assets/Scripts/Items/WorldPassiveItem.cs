using UnityEngine;

public class WorldPassiveItem : MonoBehaviour
{
    [SerializeField] private PassiveItemObject item;

    public void SetItem(PassiveItemObject item)
    {
        this.item = item;
    }

    //State
    private void OnTriggerEnter(Collider other)
    {
        //Check if player
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Player player)) return;

        //Add item to player
        player.Loadout.AddToPassiveItems(item.Logic);

        //Destroy self
        Destroy(gameObject);
    }
}
