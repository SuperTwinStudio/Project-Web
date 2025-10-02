using System;
using System.Collections.Generic;
using Botpa;
using UnityEngine;

public class Loadout : MonoBehaviour, ISavable {

    //Events
    public delegate void ClassChanged(PlayerClass oldClass, PlayerClass newClass);

    //Classes
    [Header("Classes")]
    [SerializeField] private List<PlayerClass> classes = new();

    private event ClassChanged OnClassChanged;

    public PlayerClass CurrentClass { get; private set; }

    //Money
    public int Money { get; private set; }

    //Treasures
    private readonly SerializableDictionary<Item, int> Treasures = new();


    //Testing
    private void Start() {
        SelectClass(classes[0].Item);
        //SelectClass(classes[1].Item);
    }

    //Classes
    private PlayerClass GetClass(Item item) {
        foreach (var c in classes) {
            //Check class item
            if (c.Item != item) continue;

            //Found class -> Return it
            return c;
        }

        //Not found
        return null;
    }

    public void SelectClass(Item item) {
        //Hide previous class
        if (CurrentClass) CurrentClass.Show(false);

        //Select class
        var oldClass = CurrentClass;
        CurrentClass = GetClass(item);

        //Show new class
        if (CurrentClass) CurrentClass.Show(true);

        //Call event
        OnClassChanged?.Invoke(oldClass, CurrentClass);
    }

    public void AddOnClassChanged(ClassChanged action) {
        OnClassChanged += action;
    }

    public void RemoveOnClassChanged(ClassChanged action) {
        OnClassChanged -= action;
    }

    //Using
    public void UsePrimary() {
        if (CurrentClass) CurrentClass.UsePrimary();
    }

    public void UseSecondary() {
        if (CurrentClass) CurrentClass.UseSecondary();
    }

    //Saving
    public string OnSave() {
        //Create treasures dictionary
        var treasures = new SerializableDictionary<string, int>();
        foreach (var pair in Treasures) treasures.Add(pair.Key.FileName, pair.Value);

        //Create save
        var save = new LoadoutInfo() {
            //Weapon
            currentClass = CurrentClass ? CurrentClass.Item.FileName : "",
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

        //Load class
        SelectClass(Item.GetItemFromName(save.currentClass));

        //Load money
        Money = save.money;

        //Load treasures
        Treasures.Clear();
        foreach (var pair in save.treasures) Treasures.Add(Item.GetItemFromName(pair.Key), pair.Value);
    }

    [Serializable]
    private class LoadoutInfo {

        //Classes
        public string currentClass;

        //Money
        public int money = 0;

        //Treasures
        public SerializableDictionary<string, int> treasures = new();
    
    }

}
