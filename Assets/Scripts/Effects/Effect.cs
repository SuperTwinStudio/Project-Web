using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable, CreateAssetMenu(fileName = "Effect", menuName = "Paper/Effect")]
public class Effect : ScriptableObject {

    //Info
    [SerializeField] private bool _show = true;
    [SerializeField] private Sprite _icon;
    [SerializeField] private LocalizedString _name;
    [SerializeField] private EffectAction _action;

    private readonly static Dictionary<string, Effect> cache = new();

    public bool Show => _show;
    public Sprite Icon => _icon;
    public string Name => _name.GetLocalizedString();
    public EffectAction Action => _action;
    public string FileName => name; //"name" is the name of the object, aka the file


    //Get effect
    public static Effect GetFromName(string name) {
        //Invalid name
        if (string.IsNullOrEmpty(name)) return null;

        //Check if its in cache
        if (cache.ContainsKey(name)) return cache[name];

        //Load from resources
        var effect = Resources.Load<Effect>($"Effects/{name}");
        cache[name] = effect;
        return effect;
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