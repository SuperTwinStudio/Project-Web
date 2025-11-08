using UnityEngine;

public class PIAerodynamic : PassiveItem {

    [SerializeField] private Effect aerodynamic1Effect;
    [SerializeField] private Effect aerodynamic2Effect;

    public override void OnPickup(Player player, int itemCount) {
        if (itemCount <= 1) 
            player.AddEffect(aerodynamic1Effect);
            //player.SpeedMultiplier += .1f;
        else 
            player.AddEffect(aerodynamic2Effect);
            //player.SpeedMultiplier += .05f;
    }

}
