using UnityEngine;

public class PIUnfoldedSheet : PassiveItem
{
    public override void OnDeathHook(Player player, int itemCount)
    {
        player.IsAlive = true;
        player.Heal(player.HealthMax / 2);

        player.Loadout.QueuePassiveItemRemoval(this);
    }
}
