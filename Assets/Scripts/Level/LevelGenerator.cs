using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private Transform m_LevelContainer = null;
    private bool m_Generated = false;

    private int[,] m_GenerationGrid;

    public void GenerateLevel(LevelDefinition def)
    {
		m_LevelContainer = GameObject.FindGameObjectWithTag("LevelContainer").transform;

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
        if (IsNeighbourAvailable(new Vector2Int(mid, mid), (int)Directions.UP, def.MapSize) &&
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
        List<Vector2Int> endRooms = new List<Vector2Int>();
        Room[,] roomObjectMap = new Room[def.MapSize, def.MapSize];

        for (int x = 0; x < def.MapSize; x++)
        {
            for (int y = 0; y < def.MapSize; y++)
            {
                if (m_GenerationGrid[x, y] == 0)
                {
                    roomObjectMap[x, y] = null;
                    continue;
                }

                if (GetRoomFlag(m_GenerationGrid[x, y], RoomFlags.IS_END))
                {
                    roomObjectMap[x, y] = null;
                    endRooms.Add(new Vector2Int(x, y));
                }
                else if (GetRoomFlag(m_GenerationGrid[x, y], RoomFlags.IS_START))
                {
                    GameObject room = GameObject.Instantiate(def.StartRoom, GetRoomScenePos(new Vector2Int(x, y), def.RoomSize), Quaternion.identity, m_LevelContainer);
                    roomObjectMap[x, y] = room.GetComponent<Room>();

                    startRoomPos = new Vector2(x * def.RoomSize, -y * def.RoomSize);
                }
                else
                {
                    int roomId = Random.Range(0, def.StandardRooms.Length);
                    GameObject room = GameObject.Instantiate(def.StandardRooms[roomId], GetRoomScenePos(new Vector2Int(x, y), def.RoomSize), Quaternion.identity, m_LevelContainer);
                    roomObjectMap[x, y] = room.GetComponent<Room>();
                }
            }
        }

        SpawnEndRooms(def, endRooms);

        for (int x = 0; x < def.MapSize; x++)
        {
            for (int y = 0; y < def.MapSize; y++)
            {
                if (roomObjectMap[x, y] == null) continue;
                roomObjectMap[x, y].InitializeDoors(m_GenerationGrid[x, y]);
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

    private void SpawnEndRooms(LevelDefinition def, List<Vector2Int> endRooms)
    {
        // Find furthest room to spawn a boss room
        int furthestRoom = -1;
        float furthestDistance = -1;
        int mid = def.MapSize / 2;
        for (int i = 0; i < endRooms.Count; i++)
        {
            float distance = Vector2.Distance(endRooms[i], new Vector2(mid, mid));
            if (Mathf.Abs(distance) > furthestDistance)
            {
                furthestDistance = distance;
                furthestRoom = i;
            }
        }

        int bossRoom = Random.Range(0, def.BossRooms.Length);
        Vector2Int bossPos = endRooms[furthestRoom];
        GameObject room = GameObject.Instantiate(def.BossRooms[bossRoom], GetRoomScenePos(bossPos, def.RoomSize), Quaternion.identity, m_LevelContainer);
        room.GetComponent<Room>().InitializeDoors(m_GenerationGrid[bossPos.x, bossPos.y]);

        // Remove boss room from further processing
        endRooms.RemoveAt(furthestRoom);

        // Create treasure rooms
        if (def.TreasureRooms.Length > 0)
        {
            int treasureCount = Random.Range(2, 5);
            for (int i = 0; i < treasureCount; i++)
            {
                int rand = Random.Range(0, endRooms.Count);
                if (endRooms.Count == 0) break;
                Vector2Int selRoom = endRooms[rand];

                int roomObj = Random.Range(0, def.TreasureRooms.Length);
                room = GameObject.Instantiate(def.TreasureRooms[roomObj], GetRoomScenePos(selRoom, def.RoomSize), Quaternion.identity, m_LevelContainer);
                room.GetComponent<Room>().InitializeDoors(m_GenerationGrid[selRoom.x, selRoom.y]);

                endRooms.RemoveAt(rand);
            }
        }

        // Create item rooms
        if (def.ItemRooms.Length > 0)
        {
            int itemCount = Random.Range(1, 3);
            for (int i = 0; i < itemCount; i++)
            {
                int rand = Random.Range(0, endRooms.Count);
                if (endRooms.Count == 0) break;
                Vector2Int selRoom = endRooms[rand];

                int roomObj = Random.Range(0, def.ItemRooms.Length);
                room = GameObject.Instantiate(def.ItemRooms[roomObj], GetRoomScenePos(selRoom, def.RoomSize), Quaternion.identity, m_LevelContainer);
                room.GetComponent<Room>().InitializeDoors(m_GenerationGrid[selRoom.x, selRoom.y]);

                endRooms.RemoveAt(rand);
            }
        }

        // Generate secret rooms
        if (def.GenerateSecretRooms && def.SecretRooms.Length != 0 && endRooms.Count != 0)
        {
            int rand = Random.Range(0, 100);
            if (rand < def.SecretRoomChance)
            {
                int roomObj = Random.Range(0, def.SecretRooms.Length);
                int roomId = Random.Range(0, endRooms.Count);
                Vector2Int roomPos = endRooms[roomId];

                room = GameObject.Instantiate(def.SecretRooms[roomObj], GetRoomScenePos(roomPos, def.RoomSize), Quaternion.identity, m_LevelContainer);
                room.GetComponent<Room>().InitializeDoors(m_GenerationGrid[roomPos.x, roomPos.y]);

                endRooms.RemoveAt(roomId);

                if (!def.SecretRoomsHaveDoors)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        // Mask out the door flag
                        switch (i)
                        {
                            case (int)Directions.UP:
                                if (roomPos.y == 0) continue;
                                m_GenerationGrid[roomPos.x, roomPos.y - 1] &= 0b11111101;
                                break;

                            case (int)Directions.DOWN:
                                if (roomPos.y == def.MapSize - 1) continue;
                                m_GenerationGrid[roomPos.x, roomPos.y + 1] &= 0b11111110;
                                break;

                            case (int)Directions.LEFT:
                                if (roomPos.x == 0) continue;
                                m_GenerationGrid[roomPos.x - 1, roomPos.y] &= 0b11110111;
                                break;

                            case (int)Directions.RIGHT:
                                if (roomPos.x == def.MapSize - 1) continue;
                                m_GenerationGrid[roomPos.x + 1, roomPos.y] &= 0b11111011;
                                break;
                        }
                    }
                }
            }
        }

        // Purge remaining end rooms
        foreach (Vector2Int roomPos in endRooms)
        {
            for (int i = 0; i < 4; i++)
            {
                // Mask out the door flag
                switch (i)
                {
                    case (int)Directions.UP:
                        if (roomPos.y == 0) continue;
                        m_GenerationGrid[roomPos.x, roomPos.y - 1] &= 0b11111101;
                        break;

                    case (int)Directions.DOWN:
                        if (roomPos.y == def.MapSize - 1) continue;
                        m_GenerationGrid[roomPos.x, roomPos.y + 1] &= 0b11111110;
                        break;

                    case (int)Directions.LEFT:
                        if (roomPos.x == 0) continue;
                        m_GenerationGrid[roomPos.x - 1, roomPos.y] &= 0b11110111;
                        break;

                    case (int)Directions.RIGHT:
                        if (roomPos.x == def.MapSize - 1) continue;
                        m_GenerationGrid[roomPos.x + 1, roomPos.y] &= 0b11111011;
                        break;
                }
            }
        }
    }

    private bool GetRoomFlag(int value, RoomFlags flag)
    {
        return ((value >> (int)flag) & 0b1) == 1;
    }

    private Vector3 GetRoomScenePos(Vector2Int roomPos, int roomSize)
    {
        return new Vector3(roomPos.x * roomSize, 0, -roomPos.y * roomSize);
    }

    private enum Directions : int
    {
        UP = 0,
        DOWN,
        LEFT,
        RIGHT
    }
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