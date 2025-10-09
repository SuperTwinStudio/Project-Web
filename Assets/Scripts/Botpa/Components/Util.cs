using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

namespace Botpa {

    public class Util : MonoBehaviour {

          /*$$$$$
         /$$__  $$
        | $$  \ $$  /$$$$$$   /$$$$$$
        | $$$$$$$$ /$$__  $$ /$$__  $$
        | $$__  $$| $$  \ $$| $$  \ $$
        | $$  | $$| $$  | $$| $$  | $$
        | $$  | $$| $$$$$$$/| $$$$$$$/
        |__/  |__/| $$____/ | $$____/
                  | $$      | $$
                  | $$      | $$
                  |__/      |_*/

        ///<summary>
        ///Changes the current window fullscreen state.
        ///</summary>
        public static void SetFullscreen(bool fullscreen) {
            if (fullscreen)
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            else 
                Screen.SetResolution((int) (Screen.width * 0.8f), (int) (Screen.height * 0.8f), false);
        }
        
        ///<summary>
        ///Quits the game regardless of if its playing inside the editor.
        ///</summary>
        public static void Quit() {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }



         /*$$$$$$$                    /$$
        |__  $$__/                   | $$
           | $$  /$$$$$$  /$$   /$$ /$$$$$$
           | $$ /$$__  $$|  $$ /$$/|_  $$_/
           | $$| $$$$$$$$ \  $$$$/   | $$
           | $$| $$_____/  >$$  $$   | $$ /$$
           | $$|  $$$$$$$ /$$/\  $$  |  $$$$/
           |__/ \_______/|__/  \__/   \__*/

        ///<summary>
        ///Logs a string to the console.
        ///</summary>
        public static void Log(string text) {
            Debug.Log(text);
        }
        
        ///<summary>
        ///Capitalizes the first letter of a string.
        ///</summary>
        public static string Capitalize(string text) {
            return text.Length > 0 ? text[0].ToString().ToUpper() + text[1..] : text;
        }
        


         /*$                                     /$$
        | $$                                    | $$
        | $$        /$$$$$$   /$$$$$$$  /$$$$$$ | $$  /$$$$$$   /$$$$$$$
        | $$       /$$__  $$ /$$_____/ |____  $$| $$ /$$__  $$ /$$_____/
        | $$      | $$  \ $$| $$        /$$$$$$$| $$| $$$$$$$$|  $$$$$$
        | $$      | $$  | $$| $$       /$$__  $$| $$| $$_____/ \____  $$
        | $$$$$$$$|  $$$$$$/|  $$$$$$$|  $$$$$$$| $$|  $$$$$$$ /$$$$$$$/
        |________/ \______/  \_______/ \_______/|__/ \_______/|______*/

        ///<summary>
        ///Localizes a string <b>key</b> using the specified localization <b>table</b>.
        ///</summary>
        public static string Localize(string key, string table = "UI") {
            return LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
        }



          /*$$$$$                  /$$ /$$
         /$$__  $$                | $$|__/
        | $$  \ $$ /$$   /$$  /$$$$$$$ /$$  /$$$$$$
        | $$$$$$$$| $$  | $$ /$$__  $$| $$ /$$__  $$
        | $$__  $$| $$  | $$| $$  | $$| $$| $$  \ $$
        | $$  | $$| $$  | $$| $$  | $$| $$| $$  | $$
        | $$  | $$|  $$$$$$/|  $$$$$$$| $$|  $$$$$$/
        |__/  |__/ \______/  \_______/|__/ \_____*/
        
        ///<summary>
        ///Transforms normalized volumes in the [0, 1] range to [-80, 0] for using in AudioMixer volume.
        ///</summary>
        public static float VolumeToDB(float volume) {
            //Basically a logaritmic easing curve that transforms [0, 1] to [-80, 0]
            return (1 - Mathf.Log10(1 + 9 * volume)) * -80;
        }

        ///<summary>
        ///Plays an AudioClip using the provided AudioSource.
        ///</summary>
        public static void PlayAudio(AudioSource source, AudioClip clip) {
            source.clip = clip;
            source.Play();
        }
        
        ///<summary>
        ///Searches from an AudioClip with the provided name in the "Resources/Audios/" folder and plays it using the provided AudioSource.
        ///</summary>
        public static void PlayAudio(AudioSource source, string name) {
            PlayAudio(source, (AudioClip) Resources.Load("Audios/" + name, typeof(AudioClip)));
        }



         /*$      /$$             /$$     /$$      
        | $$$    /$$$            | $$    | $$      
        | $$$$  /$$$$  /$$$$$$  /$$$$$$  | $$$$$$$ 
        | $$ $$/$$ $$ |____  $$|_  $$_/  | $$__  $$
        | $$  $$$| $$  /$$$$$$$  | $$    | $$  \ $$
        | $$\  $ | $$ /$$__  $$  | $$ /$$| $$  | $$
        | $$ \/  | $$|  $$$$$$$  |  $$$$/| $$  | $$
        |__/     |__/ \_______/   \___/  |__/  |_*/
        
        ///<summary>
        ///Returns the sign of <b>number</b> (1 if positive, 0 if zero, -1 if negative).
        ///</summary>
        public static float Sign(float number) {
            //The sign of the number (positive 1, negative -1, zero 0)
            return number < 0 ? -1 : (number > 0 ? 1 : 0);
        }

        ///<summary>
        ///Returns a random number ranging from <b>min</b> to <b>max</b> excluding the numbers contained in <b>exclusion</b>.
        ///</summary>
        public static int RandomRangeWithExclusion(int min, int max, int[] exclusion) {
            //Works only if there are no duplicates in the exclusion list
            Array.Sort(exclusion);
            int random = UnityEngine.Random.Range(min, max - exclusion.Length);
            int add = 0;
            for (int i = 0; i < exclusion.Length; i++) {
                if (random + add >= exclusion[i]) 
                    add++;
                else
                    break;
            }
            return random + add;
        }

        ///<summary>
        ///Rounds a number into the specified amount of decimals.
        ///</summary>
        public static float RoundDecimals(float number, int decimals) {
            float temp = 1f * decimals;
            return Mathf.Round(number * temp) / temp;
        }

        ///<summary>
        ///Remaps a number from [<b>fromMin</b>, <b>fromMax</b>] to [<b>toMin</b>, <b>toMax</b>].
        ///</summary>
        public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax) {
            //Clamp value to its range
            value = Mathf.Clamp(value, fromMin, fromMax);
            
            var fromAbs = value - fromMin;
            var fromMaxAbs = fromMax - fromMin;      
        
            var normal = fromAbs / fromMaxAbs;
    
            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;
        
            return toAbs + toMin;
        }



         /*$    /$$                      /$$
        | $$   | $$                     | $$
        | $$   | $$ /$$$$$$   /$$$$$$$ /$$$$$$    /$$$$$$   /$$$$$$   /$$$$$$$
        |  $$ / $$//$$__  $$ /$$_____/|_  $$_/   /$$__  $$ /$$__  $$ /$$_____/
         \  $$ $$/| $$$$$$$$| $$        | $$    | $$  \ $$| $$  \__/|  $$$$$$
          \  $$$/ | $$_____/| $$        | $$ /$$| $$  | $$| $$       \____  $$
           \  $/  |  $$$$$$$|  $$$$$$$  |  $$$$/|  $$$$$$/| $$       /$$$$$$$/
            \_/    \_______/ \_______/   \___/   \______/ |__/      |______*/

        ///<summary>
        ///Creates a Vector3 with all values being the specified number.
        ///</summary>
        public static Vector3 Vec3(float number) {
            return new Vector3(number, number, number);
        }

        ///<summary>
        ///Miltiplies each component of the vectors.
        ///</summary>
        public static Vector3 Multiply(Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }
        
        ///<summary>
        ///Divides each component of the vectors.
        ///</summary>
        public static Vector3 Divide(Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        ///<summary>
        ///Removes the X component of a Vector3.
        ///</summary>
        public static Vector3 RemoveX(Vector3 vector) {
            return new Vector3(0, vector.y, vector.z);
        }

        ///<summary>
        ///Removes the Y component of a Vector3.
        ///</summary>
        public static Vector3 RemoveY(Vector3 vector) {
            return new Vector3(vector.x, 0, vector.z);
        }

        ///<summary>
        ///Removes the Z component of a Vector3.
        ///</summary>
        public static Vector3 RemoveZ(Vector3 vector) {
            return new Vector3(vector.x, vector.y, 0);
        }

        ///<summary>
        ///Creates a Vector2 with all values being the specified number.
        ///</summary>
        public static Vector2 Vec2(float number) {
            return new Vector2(number, number);
        }

        ///<summary>
        ///Miltiplies each component of the vectors.
        ///</summary>
        public static Vector2 Multiply(Vector2 v1, Vector2 v2) {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }
        
        ///<summary>
        ///Divides each component of the vectors.
        ///</summary>
        public static Vector2 Divide(Vector2 v1, Vector2 v2) {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }

        ///<summary>
        ///Removes the X component of a Vector2.
        ///</summary>
        public static Vector2 RemoveX(Vector2 vector) {
            return new Vector2(0, vector.y);
        }

        ///<summary>
        ///Removes the Y component of a Vector2.
        ///</summary>
        public static Vector2 RemoveY(Vector2 vector) {
            return new Vector2(vector.x, 0);
        }



         /*$   /$$ /$$$$$$
        | $$  | $$|_  $$_/
        | $$  | $$  | $$  
        | $$  | $$  | $$  
        | $$  | $$  | $$  
        | $$  | $$  | $$  
        |  $$$$$$/ /$$$$$$
         \______/ |_____*/

        ///<summary>
        ///Returns the real height of a UI element taking childs into account.
        ///</summary>
        public static float GetRealHeight(RectTransform rect) {
            //Returns the real height of a ui element
            if (rect.anchorMax == Vector2.zero) {
                //Canvas
                return rect.GetComponent<CanvasScaler>().referenceResolution.y;
            } else if (rect.anchorMin.y == 0) {
                //Is anchored to parent
                return GetRealHeight((RectTransform) rect.parent) - rect.offsetMin.y + rect.offsetMax.y;
            } else {
                //Has actual height in pixels
                return rect.sizeDelta.y;
            }
        }

        ///<summary>
        ///Scrolls the specified ScrollRect by the specified amount.
        ///</summary>
        public static void ScrollAmount(ScrollRect scrollRect, float amount) {
            float max = Mathf.Max(scrollRect.content.sizeDelta.y - GetRealHeight((RectTransform) scrollRect.transform), 0);
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - amount / max);
        }

        ///<summary>
        ///Scrolls the specified ScrollRect to the specified position.
        ///</summary>
        public static void ScrollTo(ScrollRect scrollRect, float to) {
            scrollRect.verticalNormalizedPosition = 1 - Mathf.Clamp01(Mathf.Abs(to) / scrollRect.content.sizeDelta.y);
        }

        ///<summary>
        ///Scrolls the specified ScrollRect to the specified position taking a margin into account.
        ///</summary>
        public static void ScrollTo(ScrollRect scrollRect, float to, float margin) {
            scrollRect.verticalNormalizedPosition = 1 - Mathf.Clamp01((Mathf.Abs(to) - margin) / (scrollRect.content.sizeDelta.y - margin * 2));
        }



          /*$$$$$                                     /$$$$$$  /$$                                 /$$
         /$$__  $$                                   /$$__  $$| $$                                | $$
        | $$  \__/  /$$$$$$  /$$$$$$/$$$$   /$$$$$$ | $$  \ $$| $$$$$$$  /$$  /$$$$$$   /$$$$$$$ /$$$$$$   /$$$$$$$
        | $$ /$$$$ |____  $$| $$_  $$_  $$ /$$__  $$| $$  | $$| $$__  $$|__/ /$$__  $$ /$$_____/|_  $$_/  /$$_____/
        | $$|_  $$  /$$$$$$$| $$ \ $$ \ $$| $$$$$$$$| $$  | $$| $$  \ $$ /$$| $$$$$$$$| $$        | $$   |  $$$$$$
        | $$  \ $$ /$$__  $$| $$ | $$ | $$| $$_____/| $$  | $$| $$  | $$| $$| $$_____/| $$        | $$ /$$\____  $$
        |  $$$$$$/|  $$$$$$$| $$ | $$ | $$|  $$$$$$$|  $$$$$$/| $$$$$$$/| $$|  $$$$$$$|  $$$$$$$  |  $$$$//$$$$$$$/
         \______/  \_______/|__/ |__/ |__/ \_______/ \______/ |_______/ | $$ \_______/ \_______/   \___/ |_______/
                                                                   /$$  | $$
                                                                  |  $$$$$$/
                                                                   \_____*/

        ///<summary>
        ///Toggles if a gameobject is active.
        ///</summary>
        public static void Toggle(GameObject obj) {
            obj.SetActive(!obj.activeSelf);
        }

        ///<summary>
        ///Destroys a gameobject.
        ///</summary>
        public static void Destroy(GameObject obj) {
            GameObject.Destroy(obj);
        }
        
        ///<summary>
        ///Destroys all children of a transform.
        ///</summary>
        public static void DestroyChildren(Transform t) {
            for (int i = t.childCount - 1; i >= 0; i--) {
                if (Application.isPlaying)
                    Destroy(t.GetChild(i).gameObject); 
                else
                    DestroyImmediate(t.GetChild(i).gameObject); 
            }
        }



          /*$$$$$    /$$     /$$
         /$$__  $$  | $$    | $$
        | $$  \ $$ /$$$$$$  | $$$$$$$   /$$$$$$   /$$$$$$
        | $$  | $$|_  $$_/  | $$__  $$ /$$__  $$ /$$__  $$
        | $$  | $$  | $$    | $$  \ $$| $$$$$$$$| $$  \__/
        | $$  | $$  | $$ /$$| $$  | $$| $$_____/| $$
        |  $$$$$$/  |  $$$$/| $$  | $$|  $$$$$$$| $$
         \______/    \___/  |__/  |__/ \_______/|_*/

        ///<summary>
        ///Loads the specified scene.
        ///</summary>
        public static void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }

        ///<summary>
        ///Reloads the current scene.
        ///</summary>
        public static void ReloadScene() {
            LoadScene(SceneManager.GetActiveScene().name);
        }

    }

}

