using System.Drawing;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Doors;
    [SerializeField, Range(0, 1)] private int m_ShowActiveDoors;
    [Space]
    [SerializeField] private LayerMask m_SignLayer;
    [SerializeField] private GameObject[] m_DoorSigns;
    [SerializeField] private bool m_ShowDoorSigns;
    [Space]
    [SerializeField] private bool m_EnemyRoom;
    [SerializeField] private Transform[] m_EnemySpawnPoints;
    [Space]
    [SerializeField] private bool m_BossRoom;
    [SerializeField] private GameObject m_BossObject;
    [SerializeField] private GameObject m_Elevator;

    private int m_Flags;
    private Transform m_GeometryTransform = null;

    private bool m_FadeTrigger = false;
    private bool m_Faded = true;
    private float m_Size = 1.0f;

    private bool m_BossPresent;
    private int m_EnemyCount;

    private void Update()
    {
        if (m_Faded) return;

        if (m_FadeTrigger)
        {
            if (m_Size > 0)
            {
                m_Size -= 2 * Time.deltaTime;

                m_GeometryTransform.localScale = Vector3.one * m_Size;
            }
            else
            {
                m_GeometryTransform.gameObject.SetActive(false);
                m_Faded = true;
            }
        }
        else
        {
            if(m_Size < 1)
            {
                m_Size += 2 * Time.deltaTime;
                m_GeometryTransform.localScale = Vector3.one * m_Size;
            }
            else
            {
                if ((m_EnemyRoom && m_EnemyCount != 0) || m_BossRoom) 
                {
                    LockDoors();
                }

                m_Faded = true;
            }
        }
    }

    public void InitializeRoom(int flags, LevelDefinition def)
    {
        m_Flags = flags;
        m_GeometryTransform = transform.GetChild(0);

        for (int i = 0; i < 4; i++)
        {
            m_Doors[i].SetActive(((flags >> i) & 0b1) == m_ShowActiveDoors);

            if (m_ShowDoorSigns)
            {
                m_DoorSigns[i].SetActive(((flags >> i) & 0b1) == 1);
            }
        }

        // Check for signs
        Collider[] signs = Physics.OverlapBox(transform.position, Vector3.one * (def.RoomSize / 2), Quaternion.identity, m_SignLayer);
        foreach (Collider sign in signs)
        {
            sign.transform.parent = transform.GetChild(0);
        }

        // Spawn & initialize enemies
        if (m_EnemyRoom)
        {
            foreach (Transform spawnPoint in m_EnemySpawnPoints)
            {
                // Chance to not spawn an enemy at this point
                int skipRandom = Random.Range(0, 100);
                if (skipRandom < 10) continue;

                bool isRare = Random.Range(0, 100) < def.RareEnemyChance;
                if (isRare)
                {
                    int id = Random.Range(0, def.RareEnemies.Length);
                    Instantiate(def.RareEnemies[id], spawnPoint).GetComponent<EnemyBase>().SetRoom(this);
                }
                else
                {
                    int id = Random.Range(0, def.FodderEnemies.Length);
                    Instantiate(def.FodderEnemies[id], spawnPoint).GetComponent<EnemyBase>().SetRoom(this);
                }

                m_EnemyCount++;
            }
        }
        else if (m_BossRoom)
        {
            m_BossObject.GetComponent<EnemyBase>().SetRoom(this);
        }

        // Shrink all rooms but the starting room
        if (((flags >> (int)RoomFlags.IS_START) & 0b1) == 0) 
        {
            m_Size = 0;
            m_GeometryTransform.localScale = Vector3.one * m_Size;
        }

    }

    public void FadeIn()
    {
        m_GeometryTransform.gameObject.SetActive(true);
        m_Faded = false;
        m_FadeTrigger = false;
    }

    public void FadeOut()
    {
        m_Faded = false;
        m_FadeTrigger = true;
    }

    public void EnemyKilled()
    {
        if (m_BossRoom)
        {
            // ALLOW ACCESS TO NEXT FLOOR
            m_Elevator.SetActive(true);
            UnlockDoors();
            m_BossPresent = false;

            return;
        }

        m_EnemyCount--;

        if (m_EnemyCount == 0)
        {
            UnlockDoors();
        }
    }

    private void LockDoors()
    {
        for (int i = 0; i < 4; i++)
        {
            m_Doors[i].SetActive(true);
        }
    }

    private void UnlockDoors()
    {
        for (int i = 0; i < 4; i++)
        {
            m_Doors[i].SetActive(((m_Flags >> i) & 0b1) == m_ShowActiveDoors);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeOut();
        }
    }
}
