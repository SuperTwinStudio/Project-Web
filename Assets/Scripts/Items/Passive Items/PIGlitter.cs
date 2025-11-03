using UnityEngine;

public class PIGlitter : PassiveItem
{
    [SerializeField] private Effect m_Stun;

    public override void OnEnemyHurtHook(Player player, int itemCount, Character enemy)
    {
        if (GetScaledChance(itemCount, 15, 2)) enemy.AddEffect(m_Stun, 5);
    }
}
