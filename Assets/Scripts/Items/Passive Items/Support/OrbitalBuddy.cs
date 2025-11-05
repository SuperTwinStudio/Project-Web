using System.Collections.Generic;
using UnityEngine;

public class OrbitalBuddy : MonoBehaviour
{
    [SerializeField] private float m_RotationSpeed = 1;
    [SerializeField] private float m_CooldownMax = 1;
    [SerializeField] private float m_DetectionMax = 1;
    private Player m_Player;

    private Character m_TargetEnemy;
    private int m_Damage;
    private float m_Cooldown;
    private float m_DetectionCooldown;

    private float m_DetectionRadius;

    void Start()
    {
        m_TargetEnemy = null;
        m_Player = Game.Current.Level.Player;
        m_DetectionRadius = GetComponent<SphereCollider>().radius;
    }

    void Update()
    {
        transform.position = m_Player.transform.position;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.up * m_RotationSpeed));

        if (m_Cooldown > 0)
        {
            m_Cooldown -= Time.deltaTime;
        }
        else if (m_TargetEnemy != null)
        {
            m_Cooldown = m_CooldownMax;
            m_TargetEnemy.Damage(m_Damage, m_Player.GetComponent<Player>(), DamageType.Ranged);
        }

        if (m_DetectionCooldown > 0)
        { 
            m_DetectionCooldown -= Time.deltaTime; 
        }
        else
        {
            m_DetectionCooldown = m_DetectionMax;

            Collider[] colliders = Physics.OverlapSphere(transform.position, m_DetectionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Debug.Log($"Added {collider.name}");
                    m_TargetEnemy = collider.GetComponent<Character>();
                }
            }
        }
    }

    public void UpdateItemCount(int itemCount)
    {
        // 50% of base damage + 25% for each extra item
        m_Damage = (30 * (50 + (25 * (itemCount - 1)))) / 100;
    }
}
