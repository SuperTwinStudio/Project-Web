using UnityEngine;

public class PIPaperFan : PassiveItem
{
    [SerializeField] private GameObject m_Projectile;

    public override void OnDashHook(Player player, int itemCount, Vector3 direction)
    {
        Instantiate(m_Projectile, player.transform.position + Vector3.up, Quaternion.LookRotation(direction));
    }
}
