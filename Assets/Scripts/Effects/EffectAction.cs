using System;
using UnityEngine;

public enum EffectType { 
    None,       //Nothing, used for counting stuff like gauntlet passive
    Damage,     //Damage per second
    Heal,       //Healing per second
    Slowness,   //Multiplier to make move speed slower
    Fastness,   //Multiplier to make move speed faster
    Weakness,   //Multiplier for damage damage dealt on self
    Strength,   //Multiplier for damage damage dealt on others
    MaxHealth   //Extra health
}

[Serializable]
public class EffectAction {

    [SerializeField] private EffectType _type = EffectType.Damage;
    [SerializeField, Min(0)] private float _value = 0;

    public EffectType Type => _type;
    public float Value => _value;

}
