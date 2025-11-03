using UnityEngine;

public class PIKindling : PassiveItem
{
    [SerializeField] private Effect m_Burn;

    public override void OnEnemyHurtHook(Player player, int itemCount, Character enemy)
    {
        if(GetScaledChance(itemCount, 15, 5)) enemy.AddEffect(m_Burn, 10);
    }
}
