using System;
using UnityEngine;
using UnityEngine.Localization;

[Serializable, CreateAssetMenu(fileName = "Item", menuName = "Paper/Item")]
public class Item : ScriptableObject {

    //Info
    [SerializeField] private Sprite _icon;
    [SerializeField] private LocalizedString _name;
    [SerializeField, Min(0)] private int _value;

    public Sprite Icon => _icon;
    public string Name => _name.GetLocalizedString();
    public int Value => _value;
    public string FileName => name; //"name" is the name of the object, aka the file


    //Get item
    public static Item GetFromName(string name) {
        //Invalid name
        if (string.IsNullOrEmpty(name)) return null;

        //Look for item in resources
        return Resources.Load<Item>($"Items/{name}");
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
