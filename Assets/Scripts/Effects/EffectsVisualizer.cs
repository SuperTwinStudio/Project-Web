using System.Collections.Generic;
using Botpa;
using UnityEngine;

public class EffectsVisualizer : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float effectWidth = 0.5f;

    private Transform effectsParent;
    private IReadOnlyDictionary<Effect, (float, int)> effects;



    //State
    public void Init(Character character, IReadOnlyDictionary<Effect, (float, int)> effects) {
        //Get transform
        if (!effectsParent) effectsParent = transform;

        //Update effects list
        this.effects = effects;

        //Update visualization
        character.AddOnEffectsUpdated(OnEffectsUpdated);
        OnEffectsUpdated();
    }

    //Effects
    private void OnEffectsUpdated() {
        //Empty transform
        Util.DestroyChildren(effectsParent);

        //Effects count
        List<Transform> effectTransforms = new();

        //Add new effects
        foreach (var pair in effects) {
            //Get effect
            Effect effect = pair.Key;

            //Check if should be shown
            if (!effect.Show) continue;
            
            //Get end timestamp & level
            (float endTimestamp, int level) = pair.Value;

            //Create effect visualizer
            GameObject obj = Instantiate(effectPrefab, effectsParent);
            obj.GetComponent<EffectVisualizer>().Visualize(effect, level);
            effectTransforms.Add(obj.transform);
        }

        //Reposition effects
        int count = effectTransforms.Count;
        if (effectTransforms.Count > 1) {
            float side = count * effectWidth / 2;
            for (int i = 0; i < count; i++) {
                float pos = Util.Remap(i, 0, count - 1, -side, side);
                effectTransforms[i].localPosition = new Vector3(pos, 0, 0);
            }
        }
    }

}
