using TMPro;
using UnityEngine;

public class EffectVisualizer : MonoBehaviour {
    
    //Components
    [Header("Components")]
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private TMP_Text text;
    

    //Effect
    public void Visualize(Effect effect, int level) {
        icon.sprite = effect.Icon;
        text.SetText(effect.HasLevels ? $"{level}" : "");
    }

}
