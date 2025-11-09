using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Attributes")]
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

    [Header("Minimap")]
    [SerializeField] private GameObject m_MinimapUnvisited;
    [SerializeField] private GameObject m_MinimapVisited;
    [Space]
    [SerializeField] private GameObject[] m_MinimapDoors;

    private int m_Flags;
    private Transform m_GeometryTransform = null;

    private bool m_FadeTrigger = false;
    private bool m_Faded = true;
    private float m_Size = 1.0f;

    private readonly List<EnemyBase> m_enemies = new();
    private bool m_BossPresent;
    private int m_EnemyCount;

    private void Start()
    {
        m_BossPresent = m_BossRoom;
    }

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
                if ((m_EnemyRoom && m_EnemyCount != 0) || (m_BossRoom && m_BossPresent)) 
                {
                    LockDoors();
                }

                m_Faded = true;
            }
        }
    }

    private void InitializeEnemy(GameObject obj)
    {
        EnemyBase enemy = obj.GetComponent<EnemyBase>();
        enemy.SetRoom(this);
        m_enemies.Add(enemy);
    }

    public void InitializeRoom(int flags, LevelDefinition def)
    {
        m_Flags = flags;
        m_GeometryTransform = transform.GetChild(0);

        for (int i = 0; i < 4; i++)
        {
            m_Doors[i].SetActive(((flags >> i) & 0b1) == m_ShowActiveDoors);
            m_MinimapDoors[i].SetActive(((flags >> i) & 0b1) == 0);

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
                    InitializeEnemy(Instantiate(def.RareEnemies[id], spawnPoint));
                }
                else
                {
                    int id = Random.Range(0, def.FodderEnemies.Length);
                    InitializeEnemy(Instantiate(def.FodderEnemies[id], spawnPoint));
                }

                m_EnemyCount++;
            }
        }
        else if (m_BossRoom)
        {
            InitializeEnemy(m_BossObject);
        }

        // Shrink all rooms but the starting room
        if (((flags >> (int)RoomFlags.IS_START) & 0b1) == 0) 
        {
            m_Size = 0;
            m_GeometryTransform.localScale = Vector3.one * m_Size;

            m_MinimapUnvisited.SetActive(true);
            m_MinimapVisited.SetActive(false);
        }
        else
        {
            m_MinimapUnvisited.SetActive(false);
            m_MinimapVisited.SetActive(true);
        }

    }

    public void FadeIn()
    {
        m_GeometryTransform.gameObject.SetActive(true);
        m_Faded = false;
        m_FadeTrigger = false;

        m_MinimapUnvisited.SetActive(false);
        m_MinimapVisited.SetActive(true);

        //Ñapa to fix enemies spawning in the center of the room (move them to their spawn point again)
        foreach (var enemy in m_enemies) if (enemy && enemy.IsAlive) enemy.transform.localPosition = Vector3.zero;
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
        for (int i = 0; i < 4; i++) {
            m_Doors[i].SetActive(true);
        }

        //Update walkable surface
        Game.Current.Level.UpdateWalkableSurface();

        //Enable enemies
        foreach (var enemy in m_enemies) enemy.SetEnabled(true);
    }

    private void UnlockDoors()
    {
        for (int i = 0; i < 4; i++) {
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
