using UnityEngine;
using System.Collections.Generic;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Botpa {

    public class Cinematic : MonoBehaviour {

        //Config
        [Header("Config")]
        [SerializeField] private TimerScale _timeScale;

        public TimerScale TimeScale  => _timeScale;

        //Autoplay
        [Header("Autoplay")]
        [SerializeField] private bool _playOnStart;
        [SerializeField, Min(0)] private float _playOnStartDelay;

        private Timer playOnStartTimer;

        public bool PlayOnStart  => _playOnStart;
        public float PlayOnStartDelay  => _playOnStartDelay;

        //Events
        [Space(10)]
        [SerializeField] private List<TurboEvent> events;

        private Coroutine playCoroutine = null;
        private float lastEventTimestamp = 0;
        private float nextEventWait = 0;

        public bool IsPlaying => playCoroutine != null;
        public bool IsPaused { get; private set; } = false;


        //State
        private void Start() {
            //Autoplay
            if (PlayOnStart) {
                //No delay -> Play
                if (PlayOnStartDelay <= 0) Play();
                //Has delay -> Wait
                else playOnStartTimer = new(TimeScale, PlayOnStartDelay);
            }
        }

        private void Update() {
            //Not waiting for autoplay
            if (playOnStartTimer == null || !playOnStartTimer.IsActive) return;

            //Autoplay wait finished
            if (playOnStartTimer.IsFinished) {
                playOnStartTimer.Reset();
                Play();
            }
        }

        //General
        public void End() {
            //Reset coroutine & resume
            playCoroutine = null;
            IsPaused = false;
        }

        public void Stop() {
            //Not playing
            if (!IsPlaying) return;

            //Stop coroutine & end cinematic
            StopCoroutine(playCoroutine);
            End();
        }

        public void Play() {
            //Stop coroutine
            Stop();

            //Start a new one
            playCoroutine = StartCoroutine(PlayCoroutine());
        }

        IEnumerator PlayCoroutine() {
            //Start events loop
            foreach (var e in events) {
                //Reset waits
                lastEventTimestamp = Timer.GetCurrentTime(TimeScale);
                nextEventWait = 0;

                //Run events
                e.Invoke();

                //Wait
                yield return new WaitUntil(() => { 
                    return (!IsPaused) && (Timer.GetCurrentTime(TimeScale) - lastEventTimestamp > nextEventWait); 
                });
            }

            //End coroutine
            End();
        }

        //Waiting
        public void Wait(float seconds) {
            //Not playing
            if (!IsPlaying) return;

            //Add seconds to wait
            nextEventWait += seconds;
        }

        public void Pause() {
            //Not playing
            if (!IsPlaying) return;

            //Pause
            IsPaused = true;
        }

        public void Resume() {
            //Resume
            IsPaused = false;
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
        [CustomEditor(typeof(Cinematic)), CanEditMultipleObjects]
        public class CinematicEditor : Editor {

            private SerializedProperty timeScale;
            private SerializedProperty playOnStart;
            private SerializedProperty playOnStartDelay;
            private SerializedProperty events;


            public void OnEnable() {
                timeScale = serializedObject.FindProperty("_timeScale");
                playOnStart = serializedObject.FindProperty("_playOnStart");
                playOnStartDelay = serializedObject.FindProperty("_playOnStartDelay");
                events = serializedObject.FindProperty("events");
            }

            public override void OnInspectorGUI() {
                //Update object
                serializedObject.Update();

                //Begin
                EditorGUI.BeginChangeCheck();

                //Config
                EditorGUILayout.PropertyField(timeScale);

                //Play on start
                EditorGUILayout.PropertyField(playOnStart);
                if (playOnStart.boolValue) EditorGUILayout.PropertyField(playOnStartDelay);

                //Events
                EditorGUILayout.PropertyField(events);

                //End
                if (EditorGUI.EndChangeCheck()) {
                    serializedObject.ApplyModifiedProperties();
                    Undo.RecordObject(target, "Modified Cinematic script (" + target.name + ")");
                }
            }
        }
        #endif
    }

}

