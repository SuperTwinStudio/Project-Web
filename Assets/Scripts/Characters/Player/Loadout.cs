using System;
using System.Collections.Generic;
using Botpa;
using UnityEngine;

public class Loadout : MonoBehaviour, ISavable {

    //Events
    public delegate void ClassChanged(Weapon oldWeapon, Weapon newWeapon);

    //Components
    [Header("Components")]
    [SerializeField] protected Player _player;

    public Player Player => _player;
    public Level Level => Player.Level;

    //Weapons
    [Header("Weapon")]
    [SerializeField] private List<Weapon> weapons = new();

    private event ClassChanged OnWeaponChanged;

    public Weapon CurrentWeapon { get; private set; }

    //Money
    public int Money { get; private set; }

    //Inventory
    private readonly SerializableDictionary<Item, int> _inventory = new();

    public int InventoryValue { get; private set; }

    public IReadOnlyDictionary<Item, int> Inventory => _inventory;

    //Upgrades
    private readonly SerializableDictionary<string, int> _upgrades = new();

    public IReadOnlyDictionary<string, int> Upgrades => _upgrades;


    //State
    private void Start() {
        //Select first weapon class if none selected
        if (!CurrentWeapon) SelectWeapon(weapons[0].Item);
    }

    //Weapons
    private Weapon GetWeapon(Item item) {
        foreach (var weapon in weapons) {
            //Check weapon item
            if (weapon.Item != item) continue;

            //Found weapon -> Return it
            return weapon;
        }

        //Not found
        return null;
    }

    private void SelectWeapon(Weapon weapon) {
        //Hide previous weapon
        if (CurrentWeapon) CurrentWeapon.Show(false);

        //Select weapon
        var oldWeapon = CurrentWeapon;
        CurrentWeapon = weapon;

        //Show new weapon
        if (CurrentWeapon) CurrentWeapon.Show(true);

        //Call event
        OnWeaponChanged?.Invoke(oldWeapon, CurrentWeapon);
    }

    public void SelectWeapon(Item item) {
        SelectWeapon(GetWeapon(item));
    }

    public void AddOnWeaponChanged(ClassChanged action) {
        OnWeaponChanged += action;
    }

    public void RemoveOnWeaponChanged(ClassChanged action) {
        OnWeaponChanged -= action;
    }

    public bool UsePrimary() {
        return CurrentWeapon && CurrentWeapon.UsePrimary();
    }

    public bool UseSecondary() {
        return CurrentWeapon && CurrentWeapon.UseSecondary();
    }

    //Upgrades
    public int GetUpgrade(string key) {
        //Not found -> Upgrade is in tier 1
        if (!Upgrades.ContainsKey(key)) return 1;

        //Found -> Return upgrade tier
        return Upgrades[key];
    }

    public void SetUpgrade(string key, int tier) {
        _upgrades[key] = Math.Max(tier, 1);
    }

    //Money
    public bool PayMoney(int amount) {
        //No enough money
        if (Money < amount) return false;

        //Pay
        Money -= amount;
        return true;
    }

    //Inventory
    public void ClearInventory() {
        //Empty dictionary
        _inventory.Clear();

        //Reset value
        InventoryValue = 0;
    }

    public void AddToInventory(Item item, int amount) {
        //Invalid item
        if (!item) return;

        //Add item
        if (Inventory.ContainsKey(item))
            _inventory[item] += amount;
        else
            _inventory[item] = amount;

        //Update value
        InventoryValue += item.Value * amount;
    }

    public int SellInventory() {
        //Add inventory value to money
        int addedValue = InventoryValue;
        Money += addedValue;

        //Clear inventory items & value
        ClearInventory();

        //Return added value
        return addedValue;
    }

    //Saving
    public string OnSave() {
        //Create inventory save
        var inventory = new SerializableDictionary<string, int>();
        foreach (var pair in Inventory) inventory.Add(pair.Key.FileName, pair.Value);

        //Create save
        var save = new LoadoutSave() {
            //Weapon
            currentWeapon = CurrentWeapon ? CurrentWeapon.Item.FileName : "",
            //Upgrades
            upgrades = _upgrades,
            //Money
            money = Money,
            //Inventory
            inventory = inventory
        };
        return JsonUtility.ToJson(save);
    }

    public void OnLoad(string saveJson) {
        //Parse save
        var save = JsonUtility.FromJson<LoadoutSave>(saveJson);

        //Load upgrades (need to do it before weapon)
        _upgrades.Clear();
        foreach (var pair in save.upgrades) _upgrades.Add(pair.Key, pair.Value);

        //Load weapon
        SelectWeapon(Item.GetFromName(save.currentWeapon));

        //Load money
        Money = save.money;

        //Load inventory
        ClearInventory();
        foreach (var pair in save.inventory) AddToInventory(Item.GetFromName(pair.Key), pair.Value);

        //Sell inventory if in lobby
        if (Level.IsLobby) {
            //Sell inventory
            int addedValue = SellInventory();

            //Show items sold animation
            if (Level.MenuManager.TryGetMenu(out GameMenu menu)) menu.ShowItemsSold(addedValue);
        }
    }

    [Serializable]
    private class LoadoutSave {

        //Weapon
        public string currentWeapon = "";

        //Upgrades
        public SerializableDictionary<string, int> upgrades = new();

        //Money
        public int money = 0;

        //Inventory
        public SerializableDictionary<string, int> inventory = new();

    }

}
