using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "PassiveItem", menuName = "Paper/Passive Item")]
public class PassiveItemObject : ScriptableObject
{
    [SerializeField] private LocalizedString _name;
    public string Name => _name.GetLocalizedString();

    public Sprite Icon;
    public ItemRarity Rarity;
    
    public PassiveItem Logic;
}

public enum ItemRarity : int
{
    COMMON = 0,
    UNCOMMON,
    RARE
}