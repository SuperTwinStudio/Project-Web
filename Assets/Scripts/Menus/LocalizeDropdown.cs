using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[RequireComponent(typeof(TMP_Dropdown))]
public class LocalizeDropdown : MonoBehaviour {
    
    //Components
    private TMP_Dropdown dropdown;

    //Values
    [Header("Values")]
    [SerializeField] private List<LocalizedString> values; 


    //State
    private void Awake() {
        //Get dropdown
        dropdown = GetComponent<TMP_Dropdown>();

        //Add locale changed event & update current dropdown values
        LocalizationSettings.SelectedLocaleChanged += UpdateDropdown;
        UpdateDropdown();
    }

    private void OnDestroy() {
        //Remove locale changed event
        LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
    }

    //Values localization
    private void UpdateDropdown(Locale newLocale) {
        UpdateDropdown();
    }

    private void UpdateDropdown() {
        int size = dropdown.options.Count;
        for (int i = 0; i < size; i++) dropdown.options[i].text = values[i].GetLocalizedString();
        dropdown.captionText.SetText(dropdown.options[dropdown.value].text);
    }

}
