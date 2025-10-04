using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour {
    
    //Item
    [Header("")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;


    //State
    public void Init(Item item, int amount) {
        icon.sprite = item.Icon;
        amountText.SetText($"{amount}");
    }

}