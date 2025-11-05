using UnityEngine;

public class PIWetSponge : PassiveItem
{
    [SerializeField] private Effect m_Water;

    public override void OnEnemyHurtHook(Player player, int itemCount, Character enemy)
    {
        if (GetScaledChance(itemCount, 15, 5)) enemy.AddEffect(m_Water, 5);
    }
}
