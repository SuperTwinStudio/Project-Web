using UnityEngine;

public class PIFountainPen : PassiveItem
{
    [SerializeField] private Effect m_Ink;

    public override void OnEnemyHurtHook(Player player, int itemCount, Character enemy)
    {
        if (GetScaledChance(itemCount, 10, 5)) enemy.AddEffect(m_Ink, 5);
    }
}
