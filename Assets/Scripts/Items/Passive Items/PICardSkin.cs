using UnityEngine;

public class PICardSkin : PassiveItem
{
    [SerializeField] private Effect cardSkinEffect;

    public override void OnPickup(Player player, int itemCount)
    {
        player.AddEffect(cardSkinEffect);
        player.Heal(cardSkinEffect.Action.Value);

        //float healthPercentage = player.GetBaseHealth() / 10; //10% of player base health is always 10
        //float health = player.GetBaseHealth() + (healthPercentage * itemCount);

        //player.SetMaxHealth(health);
        //player.Heal(healthPercentage * itemCount);
    }
}

