using Botpa;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Preferences : MonoBehaviour {
    
    //Audio
    private static AudioMixer _audioMixer;
    private static AudioMixer AudioMixer {
        get {
            if (!_audioMixer) AudioMixer = Resources.Load<AudioMixer>("AudioMixer");
            return _audioMixer;
        }
        set {
            _audioMixer = value;
        }
    }
    
    private static float _volumeMaster = -1;
    private static float _volumeMusic = -1;
    private static float _volumeSFX = -1;

    public static float VolumeMaster {
        get {
            if (_volumeMaster < 0) VolumeMaster = PlayerPrefs.GetFloat("Audio.volumeMaster", 1f);
            return _volumeMaster;
        }
        set {
            _volumeMaster = value;
            AudioMixer.SetFloat("Master", Util.VolumeToDB(value));
            PlayerPrefs.SetFloat("Audio.volumeMaster", _volumeMaster);
        }
    }
    public static float VolumeMusic {
        get {
            if (_volumeMusic < 0) VolumeMusic = PlayerPrefs.GetFloat("Audio.volumeMusic", 0.8f);
            return _volumeMusic;
        }
        set {
            _volumeMusic = value;
            AudioMixer.SetFloat("Music", Util.VolumeToDB(value));
            PlayerPrefs.SetFloat("Audio.volumeMusic", _volumeMusic);
        }
    }
    public static float VolumeSFX {
        get {
            if (_volumeSFX < 0) VolumeSFX = PlayerPrefs.GetFloat("Audio.volumeSFX", 0.8f);
            return _volumeSFX;
        }
        set {
            _volumeSFX = value;
            AudioMixer.SetFloat("SFX", Util.VolumeToDB(value));
            PlayerPrefs.SetFloat("Audio.volumeSFX", _volumeSFX);
        }
    }

    //UI (Audio)
    [Header("Audio")]
    [SerializeField] private Slider volumeMasterSlider;
    [SerializeField] private Slider volumeMusicSlider;
    [SerializeField] private Slider volumeSFXSlider;

    //UI (Game)
    [Header("Game")]
    [SerializeField] private TMP_Dropdown languageDropdown;


    //UI
    private void Awake() {
        //Game
        languageDropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);

        //Audio
        volumeMasterSlider.value = VolumeMaster;
        volumeMusicSlider.value = VolumeMusic;
        volumeSFXSlider.value = VolumeSFX;
    }

    //Init preferences
    public static void InitAudio() {
        //Assign values to a random variable so that the getters get executed & the audio mixer updates
        float f;
        f = VolumeMaster;
        f = VolumeMusic;
        f = VolumeSFX;
    }

    //Update values
    public void SetLanguage(int newLanguage) {
        //Update locale
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[newLanguage];

        //Update preferences
        PlayerPrefs.SetString("Settings.Locale", LocalizationSettings.SelectedLocale.Identifier.Code);
    }

}
