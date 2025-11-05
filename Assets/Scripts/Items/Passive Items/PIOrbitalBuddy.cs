using UnityEngine;

public class PIOrbitalBuddy : PassiveItem
{
    [SerializeField] private GameObject m_BuddyObject;

    public override void OnPickup(Player player, int itemCount)
    {
        GameObject buddy;

        if (itemCount == 1) buddy = Instantiate(m_BuddyObject);
        else buddy = GameObject.FindGameObjectWithTag("ItemBuddy");

        buddy.GetComponent<OrbitalBuddy>().UpdateItemCount(itemCount);
    }
}
