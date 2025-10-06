using System;
using System.Collections.Generic;
using Botpa;
using UnityEngine;

public class Loadout : MonoBehaviour, ISavable {

    //Events
    public delegate void ClassChanged(Weapon oldWeapon, Weapon newWeapon);

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


    //State
    private void Start() {
        //Select first weapon class if none selected
        if (!CurrentWeapon) SelectWeapon(weapons[0].Item);
    }

    //Weapon
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

    public void SellInventory() {
        Money += InventoryValue;
        ClearInventory();
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

        //Load weapon
        SelectWeapon(Item.GetFromName(save.currentWeapon));

        //Load money
        Money = save.money;

        //Load inventory
        ClearInventory();
        foreach (var pair in save.inventory) AddToInventory(Item.GetFromName(pair.Key), pair.Value);

        //Sell inventory if in lobby
        if (Game.Current.Level.IsLobby) SellInventory();
    }

    [Serializable]
    private class LoadoutSave {

        //Weapon
        public string currentWeapon = "";

        //Money
        public int money = 0;

        //Inventory
        public SerializableDictionary<string, int> inventory = new();

    }

}
