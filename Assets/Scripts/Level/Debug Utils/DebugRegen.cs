using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DebugRegen : MonoBehaviour
{
	[SerializeField] private LevelDefinition m_RoomDonor;
	[Space]
	[SerializeField] private LevelGenerator m_Generator;
	[SerializeField] private LevelDefinition m_DebugDefinition;
	[SerializeField] private TMP_InputField m_SizeField;
	[SerializeField] private TMP_InputField m_StepField;
	[SerializeField] private TMP_InputField m_DoorField;
	[Space]
	[SerializeField] private GameObject m_SecretPanel;
	[SerializeField] private TMP_InputField m_SecretField;
	[SerializeField] private Toggle m_SecretGenerationToggle;
	[SerializeField] private Toggle m_SecretDoorGenerationToggle;

	void Start()
	{
		// Copy room data from the donor
		m_DebugDefinition.StartRoom = m_RoomDonor.StartRoom;
		m_DebugDefinition.StandardRooms = m_RoomDonor.StandardRooms;
		m_DebugDefinition.TreasureRooms = m_RoomDonor.TreasureRooms;
		m_DebugDefinition.ItemRooms = m_RoomDonor.ItemRooms;
		m_DebugDefinition.BossRooms = m_RoomDonor.BossRooms;
		m_DebugDefinition.GenerateSecretRooms = m_RoomDonor.GenerateSecretRooms;
		m_DebugDefinition.SecretRoomsHaveDoors = m_RoomDonor.SecretRoomsHaveDoors;
		m_DebugDefinition.SecretRoomChance = m_RoomDonor.SecretRoomChance;
		m_DebugDefinition.SecretRooms = m_RoomDonor.SecretRooms;

		// Initialize fields
		m_SizeField.text = m_DebugDefinition.MapSize.ToString();
		m_StepField.text = m_DebugDefinition.MaxStep.ToString();
		m_DoorField.text = m_DebugDefinition.BaseDoorChance.ToString();

		// Initialize secret room panel
		if (m_DebugDefinition.SecretRooms.Length == 0)
		{
			m_SecretPanel.SetActive(false);
		}
		else
		{
			m_SecretField.text = m_DebugDefinition.SecretRoomChance.ToString();
			m_SecretGenerationToggle.isOn = m_DebugDefinition.GenerateSecretRooms;
			m_SecretDoorGenerationToggle.isOn = m_DebugDefinition.SecretRoomsHaveDoors;
		}
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

	public void ToggleSecretGeneration(bool _value)
	{
		m_DebugDefinition.GenerateSecretRooms = _value;
	}

	public void ToggleDoorGeneration(bool _value)
	{
		m_DebugDefinition.SecretRoomsHaveDoors = _value;
	}

	public void SetSecretChance()
	{
		m_DebugDefinition.SecretRoomChance = Int32.Parse(m_SecretField.text);
	}

	public void ResetSecretChance()
	{
		m_DebugDefinition.SecretRoomChance = 25;
		m_SecretField.text = "25";
	}
}
