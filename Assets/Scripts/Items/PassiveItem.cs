using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    /// <summary>
    /// Called when the player uses their primary ability
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCount"></param>
    public virtual void OnPrimaryHook(Player player, int itemCount) { }

    /// <summary>
    /// Called when the player uses their secondary ability
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCount"></param>
    public virtual void OnSecondaryHook(Player player, int itemCount) { }

    /// <summary>
    /// [UNHOOKED] Called when the player uses their passive ability
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCount"></param>
    public virtual void OnPassiveUseHook(Player player, int itemCount) { }

    /// <summary>
    /// Called when the player hurts an enemy
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCount"></param>
    /// <param name="enemy"></param>
    public virtual void OnEnemyHurtHook(Player player, int itemCount, Character enemy) { }

    /// <summary>
    /// Called when the player gets hurt
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCount"></param>
    /// <param name="enemy"></param>
    public virtual void OnHurtHook(Player player, int itemCount, Character enemy) { }

    /// <summary>
    /// [UNHOOKED] Called when the player uses their dash
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCount"></param>
    public virtual void OnDashHook(Player player, int itemCount) { }

    /// <summary>
    /// Called when the player dies
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCount"></param>
    public virtual void OnDeathHook(Player player, int itemCount) { }

    /// <summary>
    /// Called when the item is picked up
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCount"></param>
    public virtual void OnPickup(Player player, int itemCount) { }

    protected bool GetScaledChance(int itemCount, int baseChance, int chanceScaling)
    {
        int rand = Random.Range(0, 100);
        int chance = baseChance + (chanceScaling * (itemCount - 1));

        return rand < chance;
    }
}
