using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public enum ItemUse { None, Single, Infinite }

[Serializable, CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject {

    //Info
    [SerializeField] private Sprite _icon;
    [SerializeField] private LocalizedString _name;

    public Sprite Icon => _icon;
    public string Name => _name.GetLocalizedString();


    //Get item
    public static Item GetItemFromName(string name) {
        return Resources.Load<Item>($"Items/{name}");
    }

    //Dictionary support
    public override string ToString() {
        return name; //The name of the file
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
