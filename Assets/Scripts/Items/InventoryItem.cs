using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour {

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text amountText;


    //State
    public void Init(Item item, int amount) {
        icon.sprite = item.Icon;
        nameText.SetText(item.Name);
        valueText.SetText($"{item.Value}G");
        amountText.SetText($"{amount}");
    }

}