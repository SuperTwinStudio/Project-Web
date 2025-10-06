using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Paper/Level", order = 0)]
public class LevelDefinition : ScriptableObject
{
    [Header("Attributes")]
    public string Name = "";
    public int MapSize = 20;
    public int RoomSize = 1;
    public int MaxStep = 5;
    public int BaseDoorChance = 80;

    [Header("Rooms")]
    public GameObject StartRoom;
    public GameObject[] StandardRooms;
    public GameObject[] TreasureRooms;
    public GameObject[] ItemRooms;
    public GameObject[] BossRooms;
	[Space]
	public bool GenerateSecretRooms;
	public bool SecretRoomsHaveDoors;
	public int SecretRoomChance;
	public GameObject[] SecretRooms;
}
