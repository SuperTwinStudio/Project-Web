using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Game _game;

    public Game Game => _game;

    //Saving
    private const int SAVE_VERSION = 2;
    private string SavePath => $"{Application.persistentDataPath}/save.paper";
    private string Save = "{}";

    public bool SaveLoaded { get; private set; }

    public bool SaveExists => File.Exists(SavePath);



    [Header("Weapon References")]
    [SerializeField] private Item[] m_WeaponItems;


    /*public void Save()
    {
        Player player = Game.Current.Level.Player;
        Loadout loadout = player.Loadout;

        // Fetch player data
        PlayerData playerData = new PlayerData();
        playerData.Gold = loadout.Gold;
        playerData.UpgradeHealthLevel = player.GetUpgrade(PlayerUpgrade.Gramaje).Level;
        playerData.UpgradeDefenseLevel = player.GetUpgrade(PlayerUpgrade.Rugosidad).Level;

        // Fetch weapon data
        WeaponData[] weaponsData = new WeaponData[5];
        for (int i = 0; i < weaponsData.Length; i++)
        {
            Item weapon = m_WeaponItems[i];

            WeaponData weaponData = new WeaponData();
            weaponData.Id = weapon.Name;
            weaponData.Unlocked = loadout.Unlocked.Contains(weapon);
            weaponData.UpgradePrimaryLevel = loadout.GetUpgrade(loadout.GetWeapon(weapon).GetUpgradeName(WeaponAction.Primary));
            weaponData.UpgradeSecondaryLevel = loadout.GetUpgrade(loadout.GetWeapon(weapon).GetUpgradeName(WeaponAction.Secondary));
            weaponData.UpgradePassiveLevel = loadout.GetUpgrade(loadout.GetWeapon(weapon).GetUpgradeName(WeaponAction.Passive));

            weaponsData[i] = weaponData;
        }

        // Set save data
        SaveData data = new SaveData();
        data.SaveVersion = SAVE_VERSION;
        data.Player = playerData;
        data.Weapons = weaponsData;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(m_Path, json);
    }*/

    /*public void Load()
    {
        if(!SaveExists())
        {
            Debug.LogError($"Save file not found at \"{m_Path}\"");
            throw new FileNotFoundException();
        }

        Player player = Game.Current.Level.Player;
        Loadout loadout = player.Loadout;

        string json = File.ReadAllText(m_Path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Load player data
        loadout.SetGold(data.Player.Gold);
        player.LoadUpgradeSaveData(PlayerUpgrade.Gramaje, data.Player.UpgradeHealthLevel);
        player.LoadUpgradeSaveData(PlayerUpgrade.Rugosidad, data.Player.UpgradeDefenseLevel);

        // Load weapon data
        WeaponData[] weaponsData = data.Weapons;
        for (int i = 0; i < weaponsData.Length; i++)
        {
            Item weapon = m_WeaponItems[i];
            WeaponData weaponData = weaponsData[i];

            if(!weaponData.Unlocked) continue;
            loadout.UnlockWeapon(weapon);
            loadout.SetUpgrade(loadout.GetWeapon(weapon).GetUpgradeName(WeaponAction.Primary), weaponData.UpgradePrimaryLevel);
            loadout.SetUpgrade(loadout.GetWeapon(weapon).GetUpgradeName(WeaponAction.Secondary), weaponData.UpgradeSecondaryLevel);
            loadout.SetUpgrade(loadout.GetWeapon(weapon).GetUpgradeName(WeaponAction.Passive), weaponData.UpgradePassiveLevel);
        }
    }*/
}

[Serializable]
public struct PlayerData
{
    public int Gold;
    public int UpgradeHealthLevel;
    public int UpgradeDefenseLevel;
}

[Serializable]
public struct WeaponData
{
    public string Id;
    public bool Unlocked;
    public int UpgradePrimaryLevel;
    public int UpgradeSecondaryLevel;
    public int UpgradePassiveLevel;
}

[Serializable]
public struct SaveData
{
    public int SaveVersion;
    public PlayerData Player;
    public WeaponData[] Weapons;
}