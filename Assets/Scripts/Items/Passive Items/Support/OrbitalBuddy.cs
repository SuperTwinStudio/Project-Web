using System.Collections.Generic;
using UnityEngine;

public class OrbitalBuddy : MonoBehaviour
{
    [SerializeField] private float m_RotationSpeed = 1;

    private List<Character> m_EnemyQueue;
    private int m_Damage;

    void Start()
    {
        m_EnemyQueue = new List<Character>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.up * m_RotationSpeed));
    }

    public void UpdateItemCount(int itemCount)
    {
        m_Damage = 10 * (50 + (25 * itemCount)) / 100;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>() != null && !other.CompareTag("Player"))
        {
            m_EnemyQueue.Add(other.GetComponent<Character>());
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Character>() != null && !other.CompareTag("Player"))
        {
            m_EnemyQueue.Remove(other.GetComponent<Character>());
        }
    }
}
