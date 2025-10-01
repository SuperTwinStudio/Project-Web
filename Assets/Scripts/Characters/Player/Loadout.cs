using System.Collections.Generic;
using Botpa;
using UnityEngine;

public class Loadout : MonoBehaviour, ISavable {

    //Classes
    [Header("Classes")]
    [SerializeField] private List<PlayerClass> classes = new();

    private PlayerClass _currentClass;

    public PlayerClass CurrentClass { get => _currentClass; private set => _currentClass = value; }

    //Money
    private int _money = 0;

    public int Money { get => _money; private set => _money = value; }

    //Treasures
    private readonly SerializableDictionary<Item, int> Treasures = new();


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
        CurrentClass = GetClass(item);
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

    [System.Serializable]
    private class LoadoutInfo {

        //Classes
        public string currentClass;

        //Money
        public int money = 0;

        //Treasures
        public SerializableDictionary<string, int> treasures = new();
    
    }

}
