using UnityEngine;

public class WorldItem : MonoBehaviour {

    //Item
    [Header("Item")]
    [SerializeField] private bool setItemAtStart;
    [SerializeField] private Item item;
    [SerializeField, Min(1)] private int amount = 1;

    private void Start()
    {
        if(!setItemAtStart) return;

        LevelDefinition def = Game.Current.Level.Definition;
        bool rare = def.RareTreasure.Length != 0 ? Random.Range(0, 100) < def.RareTreasureChance : false;

        int itemId = rare ? Random.Range(0, def.RareTreasure.Length) : Random.Range(0, def.NormalTreasure.Length);
        SetItem(rare ? def.RareTreasure[itemId] : def.NormalTreasure[itemId]);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", item.Icon.texture);
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
