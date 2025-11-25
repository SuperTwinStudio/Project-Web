using System;
using System.Collections.Generic;
using Botpa;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHelper : MonoBehaviour {

    //Components
    private AudioSource audioSource;

    //Audios
    [SerializeField] private List<AudioPack> audioPacks = new();

    private readonly Dictionary<string, AudioPack> audios = new();


    //State
    private void Awake() {
        //Get audio source
        audioSource = GetComponent<AudioSource>();

        //Add audios to dictionary
        foreach (var pack in audioPacks) {
            //Check if name is valid
            if (string.IsNullOrEmpty(pack.Name)) {
                Debug.LogWarning($"Invalid audio pack name in \"{name}\"");
                continue;
            }

            //Check if audio already exists
            if (audios.ContainsKey(pack.Name)) {
                Debug.LogWarning($"Duplicate audio pack name \"{pack.Name}\" in \"{name}\"");
                continue;
            }

            //Save pack
            audios.Add(pack.Name, pack);
        }
    }

    //Audios
    public void Play(string packName) {
        //Check if a clip with the specified name exists
        if (!audios.TryGetValue(packName, out AudioPack pack)) {
            Debug.LogWarning($"No audio pack named \"{packName}\" exists in \"{name}\"");
            return;
        }

        //Check if pack is empty
        if (pack.Clips.IsEmpty()) {
            Debug.LogWarning($"Empty audio clips list in \"{packName}\" of \"{name}\"");
            return;
        }

        //Play audio
        audioSource.pitch = UnityEngine.Random.Range(0.92f, 1.08f);
        audioSource.PlayOneShot(pack.Clips.GetRandom(null));
    }

}

[Serializable]
public class AudioPack {
    
    [SerializeField] private string _name = "";
    [SerializeField] private List<AudioClip> _clips = new();

    public string Name => _name;
    public IReadOnlyList<AudioClip> Clips => _clips;

}