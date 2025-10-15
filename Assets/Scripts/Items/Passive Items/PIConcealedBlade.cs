using UnityEngine;

public class PIConcealedBlade : PassiveItem
{
    [SerializeField] private float m_BladeAngle;
    [SerializeField] private GameObject m_Blade;

    public override void OnPrimaryHook(Player player, int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            GameObject leftBlade = Instantiate(m_Blade);
            leftBlade.transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles - (Vector3.up * -(m_BladeAngle + i * 15)));
            leftBlade.transform.position = player.transform.position + leftBlade.transform.forward * 0.5f;

            GameObject rightBlade = Instantiate(m_Blade);
            rightBlade.transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles - (Vector3.up * (m_BladeAngle + i * 15)));
            rightBlade.transform.position = player.transform.position + rightBlade.transform.forward * 0.5f;
        }
    }
}
