using UnityEngine;

public class PIAerodynamic : PassiveItem
{
    public override void OnPickup(Player player, int itemCount)
    {
        if (itemCount > 1) player.SpeedMultiplier += .05f;
        else player.SpeedMultiplier += .1f;
    }
}
