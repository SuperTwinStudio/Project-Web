using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private LevelDefinition m_DefaultDefinition = null;
    [SerializeField] private Transform m_LevelContainer = null;
    private bool m_Generated = false;

    private int[,] m_GenerationGrid;

    void Start()
    {
        GenerateDefaultLevel();
    }

    public void GenerateDefaultLevel()
    {
        GenerateLevel(m_DefaultDefinition);
    }

    public void GenerateLevel(LevelDefinition def)
    {
        if (m_Generated) CleanLevel();

        m_GenerationGrid = new int[def.MapSize, def.MapSize];

        for (int x = 0; x < def.MapSize; x++)
        {
            for (int y = 0; y < def.MapSize; y++)
            {
                m_GenerationGrid[x, y] = 0;
            }
        }

        // Queue that stores the rooms to be generated and it's current step
        Queue<KeyValuePair<Vector2Int, int>> queue = new Queue<KeyValuePair<Vector2Int, int>>();

        // Start level generation with the starting room
        int mid = def.MapSize / 2;
        queue.Enqueue(new KeyValuePair<Vector2Int, int>(new Vector2Int(mid, mid), def.MaxStep));
        m_GenerationGrid[mid, mid] |= 0b1 << (int)RoomFlags.IS_START;

        while (queue.Count > 0)
        {
            // Get info from the queue
            KeyValuePair<Vector2Int, int> currentRoom = queue.Dequeue();
            Vector2Int room = currentRoom.Key;
            int currentStep = currentRoom.Value;

            // Flag room as end if we've run out of steps
            if (currentStep == 0) 
            {
                m_GenerationGrid[room.x, room.y] |= 0b1 << (int)RoomFlags.IS_END;
                continue;
            }

            bool doorAdded = false;
            for (int i = 0; i < 4; i++)
            {
                int rand = Random.Range(0, 100);
                int div = (currentStep == def.MaxStep) ? 1 : def.MaxStep - currentStep;

                // Randomize if we get a door or not
                if (rand > (def.BaseDoorChance / div)) continue;

                // Check if there is a room
                if (!IsNeighbourAvailable(room, i, def.MapSize)) continue;

                // Add room to queue
                Vector2Int newRoom = GetNeighbouringPosition(room, i);
                queue.Enqueue(new KeyValuePair<Vector2Int, int>(newRoom, currentStep - 1));
                m_GenerationGrid[newRoom.x, newRoom.y] |= 0b1 << (int)RoomFlags.IS_OCCUPIED;

                // Add door flags
                m_GenerationGrid[room.x, room.y] |= 0b1 << i;

                // Up and right are even (0 & 2), to get down or right we add 1 (look at room flag enum)
                int shift = (i % 2 == 0) ? i + 1 : i - 1;
                m_GenerationGrid[newRoom.x, newRoom.y] |= 0b1 << shift;

                doorAdded = true;
            }

            // Flag as end room if no doors are generated
            if (!doorAdded)
            {
                m_GenerationGrid[room.x, room.y] |= 0b1 << (int)RoomFlags.IS_END;
            }
        }

        // Nothing got generated, try again
        if(IsNeighbourAvailable(new Vector2Int(mid, mid), (int)Directions.UP, def.MapSize) &&
            IsNeighbourAvailable(new Vector2Int(mid, mid), (int)Directions.DOWN, def.MapSize) &&
            IsNeighbourAvailable(new Vector2Int(mid, mid), (int)Directions.LEFT, def.MapSize) &&
            IsNeighbourAvailable(new Vector2Int(mid, mid), (int)Directions.RIGHT, def.MapSize))
        {
            GenerateLevel(def);
            return;
        }

        m_Generated = true;

        // Generate room gameobjects
        m_LevelContainer.position = Vector3.zero;
        Vector2 startRoomPos = Vector2.zero;

        for (int x = 0; x < def.MapSize; x++)
        {
            for (int y = 0; y < def.MapSize; y++)
            {
                if (m_GenerationGrid[x, y] == 0) continue;

                if (GetRoomFlag(m_GenerationGrid[x, y], RoomFlags.IS_END)) 
                { 
                    SpawnEndRoom(def, new Vector2Int(x, y)); 
                }
                else if (GetRoomFlag(m_GenerationGrid[x, y], RoomFlags.IS_START)) 
                {
                    GameObject room = Instantiate(def.StartRoom, new Vector3(x * def.RoomSize, 0, -y * def.RoomSize), Quaternion.identity, m_LevelContainer);
                    room.GetComponent<Room>().InitializeDoors(m_GenerationGrid[x, y]);

                    startRoomPos = new Vector2(x * def.RoomSize, -y * def.RoomSize);
                }
                else
                {
                    // TO-DO: Randomize standard rooms
                    GameObject room = Instantiate(def.StandardRooms[0], new Vector3(x * def.RoomSize, 0, -y * def.RoomSize), Quaternion.identity, m_LevelContainer);
                    room.GetComponent<Room>().InitializeDoors(m_GenerationGrid[x, y]);
                }
            }
        }

        // Center level at 0,0
        m_LevelContainer.position = new Vector3(-startRoomPos.x, 0, -startRoomPos.y);
    }

    private void CleanLevel()
    {
        foreach (Transform room in m_LevelContainer)
        {
            GameObject.Destroy(room.gameObject);
        }

        m_Generated = false;
    }

    private bool IsNeighbourAvailable(Vector2Int room, int dir, int mapSize)
    {
        switch (dir) 
        {
            case (int)Directions.UP:
                if (room.y == 0) return false;
                return m_GenerationGrid[room.x, room.y - 1] == 0;

            case (int)Directions.DOWN:
                if ((room.y + 1) == mapSize) return false;
                return m_GenerationGrid[room.x, room.y + 1] == 0;

            case (int)Directions.LEFT:
                if (room.x == 0) return false;
                return m_GenerationGrid[room.x - 1, room.y] == 0;

            case (int)Directions.RIGHT:
                if ((room.x + 1) == mapSize) return false;
                return m_GenerationGrid[room.x + 1, room.y] == 0;
        }

        return false;
    }

    private Vector2Int GetNeighbouringPosition(Vector2Int room, int dir)
    {
        switch (dir)
        {
            case (int)Directions.UP:
                return new Vector2Int(room.x, room.y - 1);

            case (int)Directions.DOWN:
                return new Vector2Int(room.x, room.y + 1);

            case (int)Directions.LEFT:
                return new Vector2Int(room.x - 1, room.y);

            case (int)Directions.RIGHT:
                return new Vector2Int(room.x + 1, room.y);
        }

        return Vector2Int.zero;
    }

    private void SpawnEndRoom(LevelDefinition def, Vector2Int roomPos)
    {
        // TO-DO: Add end room criteria
        GameObject room = Instantiate(def.TreasureRooms[0], new Vector3(roomPos.x * def.RoomSize, 0, -roomPos.y * def.RoomSize), Quaternion.identity, m_LevelContainer);
        room.GetComponent<Room>().InitializeDoors(m_GenerationGrid[roomPos.x, roomPos.y]);
    }

    private bool GetRoomFlag(int value, RoomFlags flag) 
    {
        return ((value >> (int)flag) & 0b1) == 1;
    }

    private enum Directions : int
    {
        UP = 0,
        DOWN,
        LEFT,
        RIGHT
    }

    public enum RoomFlags : int
    {
        HAS_UP = 0,
        HAS_DOWN,
        HAS_LEFT,
        HAS_RIGHT,
        IS_START,
        IS_END,
        IS_OCCUPIED
    }
}
