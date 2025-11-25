using System;
using Botpa;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Preferences : MonoBehaviour {

    //Audio
    private static AudioMixer _audioMixer;
    private static readonly FloatPreference volumeMasterPreference = new("Settings.VolumeMaster", 1.0f, (value) => AudioMixer.SetFloat("Master", Util.VolumeToDB(value)));
    private static readonly FloatPreference volumeMusicPreference = new("Settings.VolumeMusic", 0.8f, (value) => AudioMixer.SetFloat("Music", Util.VolumeToDB(value)));
    private static readonly FloatPreference volumeSFXPreference = new("Settings.VolumeSFX", 0.8f, (value) => AudioMixer.SetFloat("SFX", Util.VolumeToDB(value)));

    private static AudioMixer AudioMixer {
        get {
            if (!_audioMixer) AudioMixer = Resources.Load<AudioMixer>("AudioMixer");
            return _audioMixer;
        }
        set => _audioMixer = value;
    }

    public static float VolumeMaster { get => volumeMasterPreference.Value; set => volumeMasterPreference.Value = value; }
    public static float VolumeMusic { get => volumeMusicPreference.Value; set => volumeMusicPreference.Value = value; }
    public static float VolumeSFX { get => volumeSFXPreference.Value; set => volumeSFXPreference.Value = value; }

    //Game
    private static readonly BoolPreference showAOE = new("Settings.ShowAOE", true);
    private static readonly IntPreference quality = new("Settings.Quality", 0);

    public static bool ShowAOE { get => showAOE.Value; set => showAOE.Value = value; }
    public static int Quality { get => quality.Value; set => quality.Value = value; }

    //UI (Audio)
    [Header("Audio")]
    [SerializeField] private Slider volumeMasterSlider;
    [SerializeField] private Slider volumeMusicSlider;
    [SerializeField] private Slider volumeSFXSlider;

    //UI (Game)
    [Header("Game")]
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private Toggle AOEToggle;
    [SerializeField] private TMP_Dropdown qualityDropdown;


    //UI
    private void Awake() {
        //Audio
        volumeMasterSlider.value = VolumeMaster;
        volumeMusicSlider.value = VolumeMusic;
        volumeSFXSlider.value = VolumeSFX;

        //Game
        languageDropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        AOEToggle.isOn = ShowAOE;
        qualityDropdown.value = QualitySettings.GetQualityLevel();
    }

    //Init preferences
    public static void Init() {
        //Assign values to a random variable so that the getters get executed & the audio mixer updates
        float f;
        f = VolumeMaster;
        f = VolumeMusic;
        f = VolumeSFX;

        //Set quality level
        SetQuality(Quality);
    }

    //Update values
    public static void SetLanguage(int newLanguage) {
        //Update locale
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[newLanguage];

        //Update preferences
        PlayerPrefs.SetString("Settings.Locale", LocalizationSettings.SelectedLocale.Identifier.Code);
    }

    public static void SetQuality(int newQuality) {
        //Update quality
        QualitySettings.SetQualityLevel(newQuality);
        Quality = QualitySettings.GetQualityLevel();
    }


    //Preferences
    public abstract class Preference<T> {

        //State
        private bool isLoaded = false;

        //Info
        private T value;
        protected readonly string key;
        protected readonly T defaultValue;
        protected Action<T> onValueChanged;

        public T Value {
            get {
                //Init
                if (!isLoaded) {
                    value = LoadValue();
                    onValueChanged?.Invoke(value);
                    isLoaded = true;
                }

                //Return value
                return value;
            }
            set {
                //Save value
                this.value = value;
                SaveValue(value);
                onValueChanged?.Invoke(value);

                //Init
                isLoaded = true;
            }
        }


        //Constructor
        public Preference(string key, T defaultValue, Action<T> onValueChanged = null) {
            this.key = key;
            this.defaultValue = defaultValue;
            this.onValueChanged = onValueChanged;
        }

        //Load/Save
        protected abstract T LoadValue();

        protected abstract void SaveValue(T value);

    }

    public class FloatPreference : Preference<float> {

        //Constructor
        public FloatPreference(string key, float defaultValue, Action<float> onValueChanged = null) : base(key, defaultValue, onValueChanged) {}

        //Load/Save
        protected override float LoadValue() {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        protected override void SaveValue(float value) {
            PlayerPrefs.SetFloat(key, value);
        }

    }

    public class IntPreference : Preference<int> {

        //Constructor
        public IntPreference(string key, int defaultValue, Action<int> onValueChanged = null) : base(key, defaultValue, onValueChanged) {}

        //Load/Save
        protected override int LoadValue() {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        protected override void SaveValue(int value) {
            PlayerPrefs.SetInt(key, value);
        }

    }

    public class StringPreference : Preference<string> {

        //Constructor
        public StringPreference(string key, string defaultValue, Action<string> onValueChanged = null) : base(key, defaultValue, onValueChanged) {}

        //Load/Save
        protected override string LoadValue() {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        protected override void SaveValue(string value) {
            PlayerPrefs.SetString(key, value);
        }

    }

    public class BoolPreference : Preference<bool> {

        //Constructor
        public BoolPreference(string key, bool defaultValue, Action<bool> onValueChanged = null) : base(key, defaultValue, onValueChanged) {}

        //Load/Save
        protected override bool LoadValue() {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        protected override void SaveValue(bool value) {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

    }

}
