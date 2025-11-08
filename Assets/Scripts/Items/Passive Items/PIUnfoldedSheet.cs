public class PIUnfoldedSheet : PassiveItem
{
    public override void OnDeathHook(Player player, int itemCount)
    {
        player.Revive(player.HealthMax / 2);
        player.Loadout.RemovePassiveItem(this);
    }
}
