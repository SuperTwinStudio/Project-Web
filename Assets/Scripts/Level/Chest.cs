using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject m_ItemPrefab;
    private Item m_Item;

    void Start()
    {
        LevelDefinition def = Game.Current.Level.Definition;
        bool rare = def.RareTreasure.Length != 0 ? Random.Range(0, 100) < def.RareTreasureChance : false;

        int itemId = rare ? Random.Range(0, def.RareTreasure.Length) : Random.Range(0, def.NormalTreasure.Length);
        m_Item = rare ? def.RareTreasure[itemId] : def.NormalTreasure[itemId];
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(m_ItemPrefab, transform.position + (Vector3.up * 0.5f), m_ItemPrefab.transform.rotation).GetComponent<WorldItem>().SetItem(m_Item);
            Destroy(gameObject);
        }
    }
}
