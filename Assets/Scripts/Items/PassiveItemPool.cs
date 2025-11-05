using UnityEngine;

[CreateAssetMenu(fileName = "Passive Item Pool", menuName = "Paper/Passive Item Pool")]
public class PassiveItemPool : ScriptableObject
{
    public PassiveItemObject[] CommonItems;
    public int CommonItemChance = 60;

    public PassiveItemObject[] UncommonItems;
    public int UncommonItemChance = 25;

    public PassiveItemObject[] RareItems;
    public int RareItemChance = 15;
}
