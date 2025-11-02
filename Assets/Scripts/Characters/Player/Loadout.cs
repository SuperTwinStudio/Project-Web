using System;
using System.Collections.Generic;
using System.Linq;
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

    //Unlocked weapons
    private readonly HashSet<Item> _unlocked = new();

    public IReadOnlyCollection<Item> Unlocked => _unlocked;

    //Weapon upgrades
    private readonly SerializableDictionary<string, int> _upgrades = new();

    public IReadOnlyDictionary<string, int> Upgrades => _upgrades;

    //Money
    public int Money { get; private set; }

    //Inventory
    private readonly SerializableDictionary<Item, int> _inventory = new();

    public int InventoryValue { get; private set; }

    public IReadOnlyDictionary<Item, int> Inventory => _inventory;

    //Passive items
    private readonly SerializableDictionary<PassiveItem, int> _passiveItems = new();
    public IReadOnlyDictionary<PassiveItem, int> PassiveItems => _passiveItems;

    //State
    private void Awake() {
        //Unlock first weapon
        if (Unlocked.IsEmpty()) UnlockWeapon(weapons[0].Item);

        //Select weapon if none selected
        if (!CurrentWeapon) SelectWeapon(Unlocked.First());
    }

    //Weapons
    public Weapon GetWeapon(Item item) {
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
        //Already selected
        if (CurrentWeapon && CurrentWeapon.Item == item) return;

        //Select
        SelectWeapon(GetWeapon(item));
    }

    public void AddOnWeaponChanged(ClassChanged action) {
        OnWeaponChanged += action;
    }

    public void RemoveOnWeaponChanged(ClassChanged action) {
        OnWeaponChanged -= action;
    }

    public bool UsePrimary() {
        if(CurrentWeapon && CurrentWeapon.UsePrimary())
        {
            foreach (var item in _passiveItems)
            {
                item.Key.OnPrimaryHook(_player, item.Value);
            }

            return true;
        }

        return false;
    }

    public bool UseSecondary() {
        if (CurrentWeapon && CurrentWeapon.UseSecondary())
        {
            foreach (var item in _passiveItems)
            {
                item.Key.OnSecondaryHook(_player, item.Value);
            }

            return true;
        }

        return false;
    }

    public void OnDamageableHit(GameObject damagedObject)
    {
        Character character = damagedObject.GetComponent<Character>();
        if (character != null)
        {
            foreach (var item in _passiveItems)
            {
                item.Key.OnEnemyHurtHook(_player, item.Value, character);
            }
        }
    }

    //Unlocked weapons
    public void UnlockWeapon(Item item) {
        //Add item to unlocked weapons
        _unlocked.Add(item);
    }

    public void UnlockAllWeapons() {
        foreach (var weapon in weapons) UnlockWeapon(weapon.Item);
    }

    //Weapon ypgrades
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
    public bool Expend(int amount) {
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

    //Passive items
    public void AddToPassiveItems(PassiveItem item)
    {
        //Invalid item
        if (!item) return;

        //Add item
        if (PassiveItems.ContainsKey(item))
            _passiveItems[item]++;
        else
            _passiveItems[item] = 1;

        item.OnPickup(Player, _passiveItems[item]);
    }

    public void RemovePassiveItem(PassiveItem item)
    {
        //Invalid item or we don't have it yet
        if (!item || !PassiveItems.ContainsKey(item)) return;

        _passiveItems[item] -= 1;

        if (_passiveItems[item] == 0)
        {
            _passiveItems.Remove(item);
        }
    }

    //Saving
    public string OnSave() {
        //Create save
        var save = new LoadoutSave() {
            //Weapon
            currentWeapon = CurrentWeapon ? CurrentWeapon.Item.FileName : "",
            //Upgrades
            upgrades = _upgrades,
            //Money
            money = Money,
        };

        //Add unlocked weapons to save
        foreach (var item in Unlocked) save.unlocked.Add(item.FileName);

        //Add inventory to save
        foreach (var pair in Inventory) save.inventory.Add(pair.Key.FileName, pair.Value);

        //Return save
        return JsonUtility.ToJson(save);
    }

    public void OnLoad(string saveJson) {
        //Parse save
        var save = JsonUtility.FromJson<LoadoutSave>(saveJson);

        //Load unlocked weapons
        _unlocked.Clear();
        foreach (var itemName in save.unlocked) _unlocked.Add(Item.GetFromName(itemName));

        //Load weapon upgrades
        _upgrades.Clear();
        foreach (var pair in save.upgrades) _upgrades.Add(pair.Key, pair.Value);

        //Load money
        Money = save.money;

        //Load inventory
        ClearInventory();
        foreach (var pair in save.inventory) AddToInventory(Item.GetFromName(pair.Key), pair.Value);
        if (Level.IsLobby) {
            //In lobby -> Sell inventory
            int addedValue = SellInventory();

            //Show items sold animation
            if (Level.MenuManager.TryGetMenu(out GameMenu menu)) menu.ShowItemsSold(addedValue);
        }

        //Load weapon
        SelectWeapon(Item.GetFromName(save.currentWeapon));
    }

    [Serializable]
    private class LoadoutSave {

        //Weapon
        public string currentWeapon = "";

        //Unlocked weapons
        public List<string> unlocked = new();

        //Weapon upgrades
        public SerializableDictionary<string, int> upgrades = new();

        //Money
        public int money = 0;

        //Inventory
        public SerializableDictionary<string, int> inventory = new();

    }

}