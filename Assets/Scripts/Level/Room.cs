using System.Collections.Generic;
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
    [SerializeField] private Enemy m_Boss;
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

    //Enemies
    private readonly List<Enemy> _enemies = new();
    private bool _isBossPresent = false;

    public IReadOnlyList<Enemy> Enemies => _enemies;
    public bool IsBossPresent => _isBossPresent;
    public int EnemyCount => _enemies.Count;


    private void Start()
    {
        _isBossPresent = m_BossRoom;
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
                m_Faded = true;
            }
        }
    }

    //Enemies
    public Enemy InitializeEnemy(Enemy enemy)
    {
        enemy.SetRoom(this);
        _enemies.Add(enemy);
        return enemy;
    }

    public Enemy InitializeEnemy(GameObject obj)
    {
        return InitializeEnemy(obj.GetComponent<Enemy>());;
    }

    public void EnemyKilled(Enemy enemy)
    {
        _enemies.Remove(enemy);

        if (EnemyCount == 0)
        {
            UnlockDoors();
            
            if (m_BossRoom)
            {
                // ALLOW ACCESS TO NEXT FLOOR
                m_Elevator.SetActive(true);
                _isBossPresent = false;

                return;
            }
        }
    }

    //Room
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

                InitializeEnemy(Instantiate(def.GetRandomEnemyPrefab(), spawnPoint));
            }
        }
        else if (m_BossRoom)
        {
            InitializeEnemy(m_Boss);
        }

        // Shrink all rooms but the starting room
        if (((flags >> (int)RoomFlags.IS_START) & 0b1) == 0) 
        {
            m_Size = 0;
            m_GeometryTransform.localScale = Vector3.one * m_Size;

            m_MinimapUnvisited.SetActive(true);
            m_MinimapVisited.SetActive(false);
            m_GeometryTransform.gameObject.SetActive(false);
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

        //Ñapa to fix enemies spawning in the center of the room (move them to their spawn point again) <-payadita
        foreach (var enemy in Enemies) if (enemy && enemy.IsAlive) enemy.transform.localPosition = Vector3.zero;
    }

    public void FadeOut()
    {
        m_Faded = false;
        m_FadeTrigger = true;
    }

    public void TryLockDoors()
    {
        if ((m_EnemyRoom && EnemyCount != 0) || (m_BossRoom && IsBossPresent)) 
        {
            LockDoors();
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
        for (int i = Enemies.Count - 1; i >= 0 ; i--) Enemies[i].NotifyPlayerEnteredRoom();
    }

    private void UnlockDoors()
    {
        for (int i = 0; i < 4; i++) {
            m_Doors[i].SetActive(((m_Flags >> i) & 0b1) == m_ShowActiveDoors);
        }
    }

    //Triggers
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
