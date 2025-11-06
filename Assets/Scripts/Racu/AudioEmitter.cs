using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;




public class AudioEmitter : MonoBehaviour
{
    [HideInInspector] public List<AudioClip> clips = new List<AudioClip>();
    [HideInInspector] public List<string> names = new List<string>();
    [HideInInspector] public List<float> volumes = new List<float>();
    private AudioSource src;

    private void Start() {
        if(GetComponent<AudioSource>() != null){
            src = GetComponent<AudioSource>();
        }else{
            Debug.LogError("The gameobject: "+gameObject.name+" does not have an audio sorce");
        }
    }

    public void Stop() {
        src.Stop();
    }
    
    /// <summary>
    /// Play the sound on the 0-based index "i" and volume "vol" from 0 to 1. 
    /// </summary>
    /// <param name="i"></param>
    /// <param name="vol"></param>
    public void Play(int i, float vol){
        vol = math.clamp(vol, 0, 1);
        vol = vol * volumes[i];
        src.clip = GetAudio(i);
        src.volume = vol;
        src.Play();
    }

    public void PlayGoofy(string name){
        Play(GetIndex(name), 1);
    }

    /// <summary>
    /// Play the sound with the name "name" and volume "vol" from 0 to 1.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="vol"></param>
    public void Play(string name, float vol){
        vol = math.clamp(vol, 0, 1);
        Play(GetIndex(name), vol);
    }

    

    /// <summary>
    /// Plays a random sound from the list of names provided with a volume "vol" from 0 to 1.
    /// </summary>
    /// <param name="names"></param>
    /// <param name="vol"></param>
    public void PlayRandom(List<string> names, float vol){
        string name = names[UnityEngine.Random.Range(0, names.Count)];
        Play(name, vol);
    }

    public void PlayRandom(float vol)
    {
        int i = GetIndex(names[UnityEngine.Random.Range(0, names.Count)]);
        Play(i, volumes[i] * vol);
    }
    
    public void PlayRandomNigga(float vol)
    {
        int i = GetIndex(names[UnityEngine.Random.Range(0, names.Count)]);
        Play(i, volumes[i] * vol);
    }

    /// <summary>
    /// Plays a random sound from the list of 0-based indexes provided with a volume "vol" from 0 to 1.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="vol"></param>
    public void PlayRandom(List<int> indexes, float vol){
        int i = indexes[UnityEngine.Random.Range(0, indexes.Count)];
        Play(i, vol);
    }

    public void PlayOnPosition(int i, float vol, Vector3 pos){
        vol = math.clamp(vol, 0, 1);
        vol = vol * volumes[i];
        AudioSource.PlayClipAtPoint(GetAudio(i), pos, vol);
    }

    public void PlayOnPosition(string name, float vol, Vector3 pos){
        PlayOnPosition(GetIndex(name), vol, pos);
    }

    private AudioClip GetAudio(int i)
    {
        return clips[i];
    }

    private int GetIndex(string name){
        name = name.ToLower();
        int i = names.IndexOf(name);
        if(i == -1){
            Debug.LogError("no sound effect found with the name ("+ name +") pn object: "+ gameObject);
            return -1;
        }
        if(names.IndexOf(name, i+1) != -1) {
            Debug.LogError("two audio clips with the same name ("+ name +") detected on object: "+ gameObject );
            return -1;
        }
        return i;
    }

}

#if UNITY_EDITOR
[CustomEditor (typeof(AudioEmitter))]
public class AudioHelperEditor : Editor{


    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        if(!Application.isPlaying){

            var manager = (AudioEmitter)target;
            if(manager == null) return;
            Undo.RecordObject(manager, "Cambia AudioHelperEditor");

            var clips = manager.clips;
            var names = manager.names;
            var volumes = manager.volumes;

            

            GUILayout.BeginVertical("Box");
            GUILayout.Space(10);
            if(GUILayout.Button("-") && clips.Count > 0) clips.Remove(clips[clips.Count - 1]);
            if(GUILayout.Button("+")) clips.Add(null);
            GUILayout.Space(10);

            //Make sure clips size matches names size
            if(clips.Count != names.Count){
                //If names bigger
                if(names.Count > clips.Count){
                    //Delete unnecessary names
                    for(int i = 0; i < names.Count - clips.Count; i++){
                        names.Remove(names[names.Count - 1]);
                        volumes.Remove(volumes[volumes.Count - 1]);
                    }
                }else{  //If clips bigger
                    //Fill names with blank
                    for(int i = 0; i < clips.Count - names.Count; i++){
                        names.Add("");
                        volumes.Add(1);
                    }
                }
            }

            for(int i = 0; i < clips.Count; i++){
                GUILayout.BeginVertical("Box");

                GUILayout.Label("Sonido "+i);

                clips[i] = (AudioClip)EditorGUILayout.ObjectField("Sound Clip", clips[i], typeof(AudioClip), true);
                names[i] = EditorGUILayout.TextField("Custom Name", names[i].ToLower());
                volumes[i] = (float)EditorGUILayout.Slider(volumes[i], 0, 1);
                
                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();

        }
    }
}
#endif