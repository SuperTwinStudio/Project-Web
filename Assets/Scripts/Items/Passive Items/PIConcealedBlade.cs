using UnityEngine;

public class PIConcealedBlade : PassiveItem
{
    [SerializeField] private float m_BladeAngle;
    [SerializeField] private GameObject m_Blade;

    public override void OnPrimaryHook(Player player, int itemCount)
    {
        if (GetScaledChance(itemCount, 50, 5))
        {
            Vector3 verticalOffset = Vector3.up * 0.5f;

            GameObject leftBlade = Instantiate(m_Blade);
            leftBlade.transform.rotation = Quaternion.Euler(player.Model.transform.rotation.eulerAngles - (Vector3.up * -m_BladeAngle));
            leftBlade.transform.position = player.transform.position + leftBlade.transform.forward * 0.5f + verticalOffset;

            GameObject rightBlade = Instantiate(m_Blade);
            rightBlade.transform.rotation = Quaternion.Euler(player.Model.transform.rotation.eulerAngles - (Vector3.up * m_BladeAngle));
            rightBlade.transform.position = player.transform.position + rightBlade.transform.forward * 0.5f + verticalOffset;
        }
    }
}
