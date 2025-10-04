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

    //Treasures
    private readonly SerializableDictionary<Item, int> _treasures = new();

    public IReadOnlyDictionary<Item, int> Treasures => _treasures;


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

    public void UsePrimary() {
        if (CurrentWeapon) CurrentWeapon.UsePrimary();
    }

    public void UseSecondary() {
        if (CurrentWeapon) CurrentWeapon.UseSecondary();
    }

    //Treasures
    public int ClearTreasures() {
        //Calculate treasures value
        int value = 0;
        foreach (var treasure in Treasures.Keys) value += treasure.Value;

        //Clear treasures
        _treasures.Clear();

        //Return emptied treasures value
        return value;
    }

    public void AddTreasure(Item treasure, int amount) {
        //Invalid treasure
        if (treasure == null) return;

        //Check if treasures has treasure
        if (Treasures.ContainsKey(treasure))
            _treasures[treasure] += amount;
        else
            _treasures[treasure] = amount;
    }

    //Saving
    public string OnSave() {
        //Create treasures dictionary
        var treasures = new SerializableDictionary<string, int>();
        foreach (var pair in Treasures) treasures.Add(pair.Key.FileName, pair.Value);

        //Create save
        var save = new LoadoutInfo() {
            //Weapon
            currentWeapon = CurrentWeapon ? CurrentWeapon.Item.FileName : "",
            //Money
            money = Money,
            //Treasures
            treasures = treasures,
        };
        return JsonUtility.ToJson(save);
    }

    public void OnLoad(string saveJson) {
        //Parse save
        var save = JsonUtility.FromJson<LoadoutInfo>(saveJson);

        //Load weapon
        SelectWeapon(Item.GetItemFromName(save.currentWeapon));

        //Load money
        Money = save.money;

        //Load treasures
        ClearTreasures();
        foreach (var pair in save.treasures) AddTreasure(Item.GetItemFromName(pair.Key), pair.Value);
    }

    [Serializable]
    private class LoadoutInfo {

        //Weapon
        public string currentWeapon;

        //Money
        public int money = 0;

        //Treasures
        public SerializableDictionary<string, int> treasures = new();
    
    }

}
