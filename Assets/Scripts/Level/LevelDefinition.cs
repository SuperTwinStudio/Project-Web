using System;
using System.Linq;
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

	[Header("Lighting")]
	[ColorUsage(false, true)] public Color LightColor;
	public Cubemap reflectionMap;

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
	public EnemyWeight[] EnemiesWeights;

	[Header("Treasure")]
	public int RareTreasureChance = 25;
	public Item[] NormalTreasure;
	public Item[] RareTreasure;


    //Helpers
    public GameObject GetRandomEnemyPrefab() {
        //Claculate total weight
        float totalWeight = EnemiesWeights.Sum(e => Math.Max(0, e.weight));

        //Get random weight
        float randomWeight = (float) (new System.Random().NextDouble() * totalWeight);

        //Find enemy with random weight
        float currentWeight = 0;
        foreach (var enemy in EnemiesWeights)  {
            //Add the current item's weight to the running total
            currentWeight += Math.Max(0, enemy.weight);

            //If the random value falls within the range of this item's weight, select it
            if (randomWeight < currentWeight) return enemy.prefab;
        }

        //Return last
        return EnemiesWeights.Last().prefab;
    }

    [Serializable]
    public class EnemyWeight {

        public GameObject prefab;
        public float weight = 1;

    }

}
