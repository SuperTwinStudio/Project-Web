using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Level", menuName = "Paper/Level")]
public class LevelDefinition : ScriptableObject
{
	[Header("Attributes")]
	public string Name = "";
	public int Layer = -1;
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
	public bool GenerateSecretRooms = false;
	public bool SecretRoomsHaveDoors = true;
	public int SecretRoomChance = 10;
	public GameObject[] SecretRooms;

	[Header("Enemies")]
	public int RareEnemyChance = 25;
	public GameObject[] FodderEnemies;
	public GameObject[] RareEnemies;

	[Header("Treasure")]
	public int RareTreasureChance = 25;
	public Item[] NormalTreasure;
	public Item[] RareTreasure;
}
