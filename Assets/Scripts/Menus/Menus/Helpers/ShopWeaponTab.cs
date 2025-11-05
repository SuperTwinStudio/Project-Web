using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopWeaponTab : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private Item _item;
    [SerializeField] private Button button;
    [SerializeField] private ShopMenu shopMenu;

    public Item Item => _item;


    //UI
    public void UpdateUI(Loadout loadout) {
        button.interactable = loadout.Unlocked.Contains(Item);
    }

    public void Select() {
        shopMenu.SelectWeaponTab(Item);
    }

}
