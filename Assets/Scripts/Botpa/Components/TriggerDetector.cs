using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Botpa {

    public class TriggerDetector : MonoBehaviour {

        //Filter
        [Header("Filter")]
        [SerializeField] private List<string> tagFilter = new() { "Player" };

        private bool useTagFilter => tagFilter.Count > 0;

        //Events
        [Header("Events")]
        [SerializeField] private bool useTriggerEvent = false;
        [SerializeField] private UnityEvent<bool> onTrigger;

        [HideInInspector, SerializeField] private readonly List<GameObject> triggered = new();

        public bool isTriggered => triggered.Count > 0;


        private void OnTriggerEnter(Collider other) {
            //Tag is not included in tags list
            if (useTagFilter && !tagFilter.Contains(other.tag)) return;

            //Object is already in triggered objects list
            if (triggered.Contains(other.gameObject)) return;

            //Add object & invoke onTrigger
            triggered.Add(other.gameObject);
            if (useTriggerEvent) onTrigger.Invoke(isTriggered);
        }

        private void OnTriggerExit(Collider other) {
            //Tag is not included in tags list
            if (useTagFilter && !tagFilter.Contains(other.tag)) return;
            
            //Remove object
            triggered.Remove(other.gameObject);
            if (useTriggerEvent) onTrigger.Invoke(isTriggered);
        }



         /*$$$$$                                                     /$$
        |_  $$_/                                                    | $$
          | $$   /$$$$$$$   /$$$$$$$  /$$$$$$   /$$$$$$   /$$$$$$$ /$$$$$$    /$$$$$$   /$$$$$$
          | $$  | $$__  $$ /$$_____/ /$$__  $$ /$$__  $$ /$$_____/|_  $$_/   /$$__  $$ /$$__  $$
          | $$  | $$  \ $$|  $$$$$$ | $$  \ $$| $$$$$$$$| $$        | $$    | $$  \ $$| $$  \__/
          | $$  | $$  | $$ \____  $$| $$  | $$| $$_____/| $$        | $$ /$$| $$  | $$| $$
         /$$$$$$| $$  | $$ /$$$$$$$/| $$$$$$$/|  $$$$$$$|  $$$$$$$  |  $$$$/|  $$$$$$/| $$
        |______/|__/  |__/|_______/ | $$____/  \_______/ \_______/   \___/   \______/ |__/
                                    | $$
                                    | $$
                                    |_*/

        #if UNITY_EDITOR
        [CustomEditor(typeof(TriggerDetector)), CanEditMultipleObjects]
        public class TriggerDetectorEditor : Editor {

            private SerializedProperty tagFilter;
            private SerializedProperty useTriggerEvent;
            private SerializedProperty onTrigger;


            public void OnEnable() {
                tagFilter = serializedObject.FindProperty("tagFilter");
                useTriggerEvent = serializedObject.FindProperty("useTriggerEvent");
                onTrigger = serializedObject.FindProperty("onTrigger");
            }

            public override void OnInspectorGUI() {
                //Update object
                serializedObject.Update();

                //Begin
                EditorGUI.BeginChangeCheck();

                //Tag filter
                EditorGUILayout.PropertyField(tagFilter);

                //Events
                EditorGUILayout.PropertyField(useTriggerEvent);
                if (useTriggerEvent.boolValue) EditorGUILayout.PropertyField(onTrigger);

                //End
                if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
                Undo.RecordObject(target, "Modified TriggerDetector script (" + target.name + ")");
                }
            }
        }
        #endif
    }

}