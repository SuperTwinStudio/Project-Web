using UnityEngine;

public class PICutter : PassiveItem
{
    public override void OnPickup(Player player, int itemCount)
    {
        player.DamageMultiplier += 0.1f;
    }
}
