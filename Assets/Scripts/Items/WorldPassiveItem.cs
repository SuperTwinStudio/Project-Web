using UnityEngine;

public class WorldPassiveItem : MonoBehaviour
{
    [SerializeField] private PassiveItemPool itemPool;
    private PassiveItemObject item;

    private void Start()
    {
        int rand = Random.Range(0, 100);

        if(rand - itemPool.CommonItemChance < 0)
        {
            int itemId = Random.Range(0, itemPool.CommonItems.Length);
            SetItem(itemPool.CommonItems[itemId]);
        }
        else if (rand - itemPool.CommonItemChance - itemPool.UncommonItemChance < 0)
        {
            int itemId = Random.Range(0, itemPool.UncommonItems.Length);
            SetItem(itemPool.UncommonItems[itemId]);
        }
        else
        {
            int itemId = Random.Range(0, itemPool.RareItems.Length);
            SetItem(itemPool.RareItems[itemId]);
        }
    }

    public void SetItem(PassiveItemObject item)
    {
        this.item = item;
        gameObject.name = item.Name;

        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", item.Icon.texture);
    }

    //State
    private void OnTriggerEnter(Collider other)
    {
        //Check if player
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Player player)) return;

        //Add item to player
        player.Loadout.AddPassiveItem(item);

        //Destroy self
        Destroy(gameObject);
    }
}
