using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPassive : MonoBehaviour
{
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_AmountText;

    private PassiveItemObject m_ItemDef;
    private TMP_Text m_DescriptionBox;

    //State
    public void Init(PassiveItem item, int amount, TMP_Text descBox) {
        m_ItemDef = PassiveItemObject.GetFromName(item.name);

        m_Icon.sprite = m_ItemDef.Icon;
        m_AmountText.SetText($"{amount}");

        m_DescriptionBox = descBox;
    }

    public void OnPointerEnter()
    {
        m_DescriptionBox.text = m_ItemDef.Description;
    }

    public void OnPointerExit()
    {
        m_DescriptionBox.text = "";
    }
}
