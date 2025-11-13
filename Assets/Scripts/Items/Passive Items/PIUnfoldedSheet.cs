public class PIUnfoldedSheet : PassiveItem
{
    public override void OnDeathHook(Player player, int itemCount)
    {
        player.Revive(player.HealthMax / 2);

        //Removing it straight away will crash the game as items are still being iterated upon
        player.Loadout.QueuePassiveItemRemoval(this);
    }
}
