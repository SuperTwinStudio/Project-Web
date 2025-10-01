using UnityEngine;
using TMPro;
using System;

public class DebugRegen : MonoBehaviour
{
	[SerializeField] private LevelDefinition m_RoomDonor;
	[Space]
	[SerializeField] private LevelGenerator m_Generator;
	[SerializeField] private LevelDefinition m_DebugDefinition;
	[SerializeField] private TMP_InputField m_SizeField;
	[SerializeField] private TMP_InputField m_StepField;
	[SerializeField] private TMP_InputField m_DoorField;

	void Start()
	{
		m_SizeField.text = m_DebugDefinition.MapSize.ToString();
		m_StepField.text = m_DebugDefinition.MaxStep.ToString();
		m_DoorField.text = m_DebugDefinition.BaseDoorChance.ToString();

		m_DebugDefinition.StartRoom = m_RoomDonor.StartRoom;
		m_DebugDefinition.StandardRooms = m_RoomDonor.StandardRooms;
		m_DebugDefinition.TreasureRooms = m_RoomDonor.TreasureRooms;
		m_DebugDefinition.ItemRooms = m_RoomDonor.ItemRooms;
		m_DebugDefinition.BossRooms = m_RoomDonor.BossRooms;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.R)) Regen();
	}

	public void SetMapSize()
	{
		m_DebugDefinition.MapSize = Int32.Parse(m_SizeField.text);
	}

	public void SetMaxStep()
	{
		m_DebugDefinition.MaxStep = Int32.Parse(m_StepField.text);
	}

	public void SetDoorChance()
	{
		m_DebugDefinition.BaseDoorChance = Int32.Parse(m_DoorField.text);
	}

	public void ResetMapSize()
	{
		m_DebugDefinition.MapSize = 50;
		m_SizeField.text = "50";
	}

	public void ResetMaxStep()
	{
		m_DebugDefinition.MaxStep = 5;
		m_StepField.text = "5";
	}

	public void ResetDoorChance()
	{
		m_DebugDefinition.BaseDoorChance = 80;
		m_DoorField.text = "80";
	}

	public void Regen()
	{
		m_Generator.GenerateLevel(m_DebugDefinition);
	}
}
