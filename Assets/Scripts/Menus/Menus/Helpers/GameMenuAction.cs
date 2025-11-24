using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuAction : MonoBehaviour {

    //Components
    [SerializeField] private Image icon;
    [SerializeField] private Image cooldown;
    [SerializeField] private GameObject valueBadge;
    [SerializeField] private TMP_Text valueText;


    //Actions
    public void SetSprite(Sprite sprite) {
        icon.sprite = sprite;
    }

    public void SetCooldown(float amount) {
        cooldown.fillAmount = amount;
    }

    public void SetValue(bool show, string value = "") {
        valueBadge.SetActive(show);
        valueText.SetText(value);
    }

}
