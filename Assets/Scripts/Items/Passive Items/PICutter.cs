using UnityEngine;

public class PICutter : PassiveItem {

    [SerializeField] private Effect cutterEffect;

    public override void OnPickup(Player player, int itemCount) {
        player.AddEffect(cutterEffect);
        //player.DamageMultiplier += 0.1f;
    }

}
