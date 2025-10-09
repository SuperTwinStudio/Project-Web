using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable, CreateAssetMenu(fileName = "Item", menuName = "Paper/Item")]
public class Item : ScriptableObject {

    //Info
    [SerializeField] private Sprite _icon;
    [SerializeField] private LocalizedString _name;
    [SerializeField, Min(0)] private int _value;

    private readonly static Dictionary<string, Item> cache = new();

    public Sprite Icon => _icon;
    public string Name => _name.GetLocalizedString();
    public int Value => _value;
    public string FileName => name; //"name" is the name of the object, aka the file


    //Get item
    public static Item GetFromName(string name) {
        //Invalid name
        if (string.IsNullOrEmpty(name)) return null;

        //Check if its in cache
        if (cache.ContainsKey(name)) return cache[name];

        //Load from resources
        var item = Resources.Load<Item>($"Items/{name}");
        cache[name] = item;
        return item;
    }

    //Dictionary support
    public override string ToString() {
        return FileName;
    }

    public override bool Equals(object obj) {
        //Invalid objects
        if (obj == null || GetType() != obj.GetType()) return false;
        
        //Hash are equal
        return GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode() {
        return HashCode.Combine(_icon, _name);
    }

}
