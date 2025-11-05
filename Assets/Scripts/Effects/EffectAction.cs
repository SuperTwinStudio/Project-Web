using System;
using UnityEngine;

public enum EffectType { None, Damage, Heal, Slow }

[Serializable]
public class EffectAction {

    [SerializeField] private EffectType _type = EffectType.Damage;
    [SerializeField, Min(0)] private float _value = 0;

    public EffectType Type => _type;
    public float Value => _value;

}
