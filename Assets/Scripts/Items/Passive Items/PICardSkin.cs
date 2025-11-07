using UnityEngine;

public class PICardSkin : PassiveItem
{
    public override void OnPickup(Player player, int itemCount)
    {
        float healthPercentage = player.GetBaseHealth() / 10;
        float health = player.GetBaseHealth() + (healthPercentage * itemCount);

        player.SetMaxHealth(health);
        player.Heal(healthPercentage * itemCount);
    }
}

