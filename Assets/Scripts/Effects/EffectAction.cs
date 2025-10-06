using System;
using UnityEngine;

public enum EffectType { Damage, Heal, Slow }

[Serializable]
public class EffectAction {

    public EffectType type;
    [Min(0)] public float duration;

}
