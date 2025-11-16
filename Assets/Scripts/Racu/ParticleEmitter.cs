using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class ParticleEmitter : MonoBehaviour
{
    [HideInInspector] public List<ParticleSystem> clips = new List<ParticleSystem>();
    [HideInInspector] public List<String> names = new List<String>();
    [HideInInspector] public List<float> volumes = new List<float>();

    private void Start() {
        
    }
    
    public void Play(int i, Vector3 normal){
        Instantiate(clips[i], transform.position, Quaternion.LookRotation(normal));
        Debug.Log(Time.frameCount);
    }

    public void Play(String name, Vector3 normal){
        Play(GetIndex(name), normal);
    }

    public void PlayRandom(List<String> names, Vector3 normal){
        String name = names[UnityEngine.Random.Range(0, names.Count)];
        Play(name, normal);
    }

    public void PlayRandom(List<int> indexes, Vector3 normal){
        int i = indexes[UnityEngine.Random.Range(0, indexes.Count)];
        Play(i, normal);
    }

    public void PlayOnPosition(int i, Vector3 normal, Vector3 pos){
        Instantiate(GetClip(i), pos, Quaternion.LookRotation(normal));
    }

    public void PlayOnPosition(String name, Vector3 normal, Vector3 pos){
        PlayOnPosition(GetIndex(name), normal, pos);
    }

    private ParticleSystem GetClip(int i){
        return clips[i];
    }

    private int GetIndex(String name){
        name = name.ToLower();
        int i = names.IndexOf(name);
        if(i == -1){
            Debug.LogError("no particle effect found with the name ("+ name +") pn object: "+ gameObject);
            return -1;
        }
        if(names.IndexOf(name, i+1) != -1) {
            Debug.LogError("two particle clips with the same name ("+ name +") detected on object: "+ gameObject );
            return -1;
        }
        return i;
    }

}

#if UNITY_EDITOR
[CustomEditor (typeof(ParticleEmitter))]
public class ParticleEmitterEditor : Editor{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        // if(!Application.isPlaying){

            var manager = (ParticleEmitter)target;
            if(manager == null) return;
            Undo.RecordObject(manager, "Cambia ParticleEmitter");

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

                GUILayout.Label("Clip "+i);

                clips[i] = (ParticleSystem)EditorGUILayout.ObjectField("Clip", clips[i], typeof(ParticleSystem), true);
                names[i] = EditorGUILayout.TextField("Custom Name", names[i].ToLower());
                
                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
        // }
    }
}
#endif