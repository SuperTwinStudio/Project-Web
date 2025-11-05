using UnityEngine;

public class PIGlueStick : PassiveItem
{
    [SerializeField] private Effect m_Glue;

    public override void OnEnemyHurtHook(Player player, int itemCount, Character enemy)
    {
        if (GetScaledChance(itemCount, 10, 5)) enemy.AddEffect(m_Glue, 5);
    }
}
