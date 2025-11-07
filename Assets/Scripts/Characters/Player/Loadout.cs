using System;
using System.Collections.Generic;
using System.Linq;
using Botpa;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Loadout : MonoBehaviour, ISavable {

    //Events
    public delegate void ClassChanged(Weapon oldWeapon, Weapon newWeapon);
    public delegate void ObtainedItem(PassiveItemObject item);

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

    //Gold
    public int Gold { get; private set; }

    //Inventory
    private readonly SerializableDictionary<Item, int> _inventory = new();

    public int InventoryValue { get; private set; }

    public IReadOnlyDictionary<Item, int> Inventory => _inventory;

    //Passive items
    private readonly SerializableDictionary<PassiveItem, int> _passiveItems = new();
    private event ObtainedItem OnObtainItem;
    private List<PassiveItem> passiveItemDeletionList;

    public IReadOnlyDictionary<PassiveItem, int> PassiveItems => _passiveItems;


    //State
    private void Awake() {
        //Unlock first weapon
        if (Unlocked.IsEmpty()) UnlockWeapon(weapons[0].Item);

        //Select weapon if none selected
        if (!CurrentWeapon) SelectWeapon(Unlocked.First());

        passiveItemDeletionList = new List<PassiveItem>();
    }

    private void Update()
    {
        if (passiveItemDeletionList.Count > 0)
        {
            foreach (var item in passiveItemDeletionList)
            {
                RemovePassiveItem(item);
            }

            passiveItemDeletionList.Clear();
        }
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

    public bool UsePrimary() {
        if (CurrentWeapon && CurrentWeapon.UsePrimary()) {
            //Use item
            foreach (var item in _passiveItems) item.Key.OnPrimaryHook(_player, item.Value);

            //Used
            return true;
        }

        //Not used
        return false;
    }

    public bool UseSecondary() {
        if (CurrentWeapon && CurrentWeapon.UseSecondary()) {
            //Use item
            foreach (var item in _passiveItems) item.Key.OnSecondaryHook(_player, item.Value);

            //Used
            return true;
        }

        //Not used
        return false;
    }

    public bool Reload() {
        if (CurrentWeapon && CurrentWeapon.Reload()) {
            //Used
            return true;
        }

        //Not used
        return false;
    }

    public void OnDamageableHit(GameObject damagedObject) {
        Character character = damagedObject.GetComponent<Character>();
        if (character) {
            foreach (var item in _passiveItems) {
                item.Key.OnEnemyHurtHook(_player, item.Value, character);
            }
        }
    }

    public void AddOnWeaponChanged(ClassChanged action) {
        OnWeaponChanged += action;
    }

    public void RemoveOnWeaponChanged(ClassChanged action) {
        OnWeaponChanged -= action;
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

    //Gold
    public bool SpendGold(int amount) {
        //No enough gold
        if (Gold < amount) return false;

        //Pay
        Gold -= amount;
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
        //Add inventory value to gold
        int addedValue = InventoryValue;
        Gold += addedValue;

        //Clear inventory items & value
        ClearInventory();

        //Return added value
        return addedValue;
    }

    //Passive items
    private void RemovePassiveItem(PassiveItem item) {
        //Invalid item or we don't have it yet
        if (!item || !PassiveItems.ContainsKey(item)) return;

        _passiveItems[item] -= 1;

        if (_passiveItems[item] == 0) _passiveItems.Remove(item);
    }

    public void AddPassiveItem(PassiveItemObject item) {
        //Invalid item
        if (!item) return;

        PassiveItem logic = item.Logic;

        OnObtainItem?.Invoke(item);

        //Add item
        if (PassiveItems.ContainsKey(logic))
            _passiveItems[logic]++;
        else
            _passiveItems[logic] = 1;

        logic.OnPickup(Player, _passiveItems[logic]);
    }

    public void SilentAddPassiveItem(PassiveItemObject item)
    {
        //Invalid item
        if (!item) return;

        PassiveItem logic = item.Logic;

        //Add item
        if (PassiveItems.ContainsKey(logic))
            _passiveItems[logic]++;
        else
            _passiveItems[logic] = 1;

        logic.OnPickup(Player, _passiveItems[logic]);
    }

    public void AddOnObtainItem(ObtainedItem action) {
        OnObtainItem += action;
    }

    public void RemoveOnObtainItem(ObtainedItem action) {
        OnObtainItem -= action;
    }

    public void QueuePassiveItemRemoval(PassiveItem item) {
        passiveItemDeletionList.Add(item);
    }

    //Saving
    public string OnSave() {
        //Create save
        var save = new LoadoutSave() {
            //Weapon
            currentWeapon = CurrentWeapon ? CurrentWeapon.Item.FileName : "",
            //Upgrades
            upgrades = _upgrades,
            //Gold
            gold = Gold,
        };

        //Add unlocked weapons to save
        foreach (var item in Unlocked) save.unlocked.Add(item.FileName);

        //Add inventory to save
        foreach (var pair in Inventory) save.inventory.Add(pair.Key.FileName, pair.Value);

        //Add items to save
        foreach (var pair in PassiveItems) save.passiveItems.Add(pair.Key.name, pair.Value);

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

        //Load gold
        Gold = save.gold;

        //Load inventory
        ClearInventory();
        foreach (var pair in save.inventory) AddToInventory(Item.GetFromName(pair.Key), pair.Value);
        if (Level.IsLobby) {
            //In lobby -> Sell inventory
            int addedValue = SellInventory();

            //Show items sold animation
            if (Game.Current.MenuManager.TryGetMenu(out GameMenu menu)) menu.ShowInventorySold(addedValue);
        }

        //Load items if not in lobby
        _passiveItems.Clear();
        if (!Level.IsLobby)
        {
            foreach (var pair in save.passiveItems)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    SilentAddPassiveItem(PassiveItemObject.GetFromName(pair.Key));
                }
            }
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

        //Gold
        public int gold = 0;

        //Inventory
        public SerializableDictionary<string, int> inventory = new();

        //Items
        public SerializableDictionary<string, int> passiveItems = new();

    }

}