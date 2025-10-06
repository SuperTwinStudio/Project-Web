using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Effect", menuName = "Paper/Effect")]
public class Effect : ScriptableObject {

    //Info
    [SerializeField] private Sprite _icon;
    [SerializeField] private List<EffectAction> _effects = new();

    public Sprite Icon => _icon;
    public List<EffectAction> Effects => _effects;

}