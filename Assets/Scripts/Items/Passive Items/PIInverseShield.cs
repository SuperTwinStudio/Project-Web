using UnityEngine;

public class PIInverseShield : PassiveItem
{
    [SerializeField] private Vector3 m_BaseScale;
    [SerializeField] private GameObject m_Shield;

    public override void OnPickup(Player player, int itemCount)
    {
        if (itemCount == 1)
        {
            Instantiate(m_Shield, player.transform);

            return;
        }

        GameObject shield = GameObject.FindGameObjectWithTag("ItemShield");
        shield.transform.localScale = m_BaseScale + ((m_BaseScale / 10) * (itemCount - 1));
    }

    public override void OnHurtHook(Player player, int itemCount, Character enemy)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = enemy.transform.position;

        Vector3 direction = new Vector3(enemyPos.x - playerPos.x, enemyPos.y - playerPos.y, enemyPos.z - playerPos.z);
        float distance = Vector3.Distance(playerPos, enemyPos);

        if(Physics.Raycast(playerPos, direction, out RaycastHit hit, distance, LayerMask.NameToLayer("ItemShield")))
        {
            if (hit.transform.gameObject.CompareTag("ItemShield")) player.IgnoreNextDamage = true;
        }
    }
}
