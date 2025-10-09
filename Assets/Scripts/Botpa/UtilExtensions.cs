using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Botpa {

    public static class UtilExtensions {
    
         /*$    /$$                      /$$
        | $$   | $$                     | $$
        | $$   | $$ /$$$$$$   /$$$$$$$ /$$$$$$    /$$$$$$   /$$$$$$   /$$$$$$$
        |  $$ / $$//$$__  $$ /$$_____/|_  $$_/   /$$__  $$ /$$__  $$ /$$_____/
         \  $$ $$/| $$$$$$$$| $$        | $$    | $$  \ $$| $$  \__/|  $$$$$$
          \  $$$/ | $$_____/| $$        | $$ /$$| $$  | $$| $$       \____  $$
           \  $/  |  $$$$$$$|  $$$$$$$  |  $$$$/|  $$$$$$/| $$       /$$$$$$$/
            \_/    \_______/ \_______/   \___/   \______/ |__/      |______*/

        ///<summary>
        ///Returns true if the vector is empty.
        ///</summary>
        public static bool IsEmpty(this ref Vector3 vector) {
            return vector == Vector3.zero;
        }

        ///<summary>
        ///Returns the multiplication of each component of the vectors.
        ///</summary>
        public static Vector3 Multiply(this Vector3 v1, Vector3 v2) {
            return Util.Multiply(v1, v2);
        }

        ///<summary>
        ///Returns the division of each component of the vectors.
        ///</summary>
        public static Vector3 Divide(this Vector3 v1, Vector3 v2) {
            return Util.Divide(v1, v2);
        }

        ///<summary>
        ///Returns the vector without the X component
        ///</summary>
        public static Vector3 RemoveX(this Vector3 vector) {
            return Util.RemoveX(vector);
        }

        ///<summary>
        ///Returns the vector without the Y component.
        ///</summary>
        public static Vector3 RemoveY(this Vector3 vector) {
            return Util.RemoveY(vector);
        }

        ///<summary>
        ///Returns the vector without the Z component.
        ///</summary>
        public static Vector3 RemoveZ(this Vector3 vector) {
            return Util.RemoveZ(vector);
        }

        ///<summary>
        ///Returns true if the vector is empty.
        ///</summary>
        public static bool IsEmpty(this ref Vector2 vector) {
            return vector == Vector2.zero;
        }

        ///<summary>
        ///Returns the multiplication of each component of the vectors.
        ///</summary>
        public static Vector2 Multiply(this Vector2 v1, Vector2 v2) {
            return Util.Multiply(v1, v2);
        }

        ///<summary>
        ///Returns the division of each component of the vectors.
        ///</summary>
        public static Vector2 Divide(this Vector2 v1, Vector2 v2) {
            return Util.Divide(v1, v2);
        }

        ///<summary>
        ///Returns the vector without the X component.
        ///</summary>
        public static Vector2 RemoveX(this Vector2 vector) {
            return Util.RemoveX(vector);
        }

        ///<summary>
        ///Returns the vector without the Y component.
        ///</summary>
        public static Vector2 RemoveY(this Vector2 vector) {
            return Util.RemoveY(vector);
        }



         /*$       /$$             /$$             
        | $$      |__/            | $$             
        | $$       /$$  /$$$$$$$ /$$$$$$   /$$$$$$$
        | $$      | $$ /$$_____/|_  $$_/  /$$_____/
        | $$      | $$|  $$$$$$   | $$   |  $$$$$$ 
        | $$      | $$ \____  $$  | $$ /$$\____  $$
        | $$$$$$$$| $$ /$$$$$$$/  |  $$$$//$$$$$$$/
        |________/|__/|_______/    \___/ |______*/ 
        
        ///<summary>
        ///Returns true if the list is empty.
        ///</summary>
        public static bool IsEmpty<T>(this IReadOnlyList<T> list) {
            return list.Count == 0;
        }

        ///<summary>
        ///Swaps two items in a list.
        ///</summary>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
            (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
        }

        ///<summary>
        ///Returns true if the dictionary is empty.
        ///</summary>
        public static bool IsEmpty<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary) {
            return dictionary.Count == 0;
        }

        ///<summary>
        ///Returns the value of the specified key, creates a new one if it does not exist.
        ///</summary>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue fallback) {
            if (!dictionary.ContainsKey(key)) dictionary[key] = fallback;
            return dictionary[key];
        }

        ///<summary>
        ///Returns true if the dictionary is empty.
        ///</summary>
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection) {
            return collection.Count == 0;
        }



         /*$$$$$                                 /$$    
        |_  $$_/                                | $$    
          | $$   /$$$$$$$   /$$$$$$  /$$   /$$ /$$$$$$  
          | $$  | $$__  $$ /$$__  $$| $$  | $$|_  $$_/  
          | $$  | $$  \ $$| $$  \ $$| $$  | $$  | $$    
          | $$  | $$  | $$| $$  | $$| $$  | $$  | $$ /$$
         /$$$$$$| $$  | $$| $$$$$$$/|  $$$$$$/  |  $$$$/
        |______/|__/  |__/| $$____/  \______/    \___/  
                          | $$                          
                          | $$                          
                          |_*/

        ///<summary>
        ///Calls the function in its action.
        ///</summary>
        public static void Enable(this InputActionReference inputActionReference) {
            inputActionReference.action.Enable();
        }

        ///<summary>
        ///Calls the function in its action.
        ///</summary>
        public static void Disable(this InputActionReference inputActionReference) {
            inputActionReference.action.Disable();
        }

        ///<summary>
        ///Calls the function in its action.
        ///</summary>
        public static T ReadValue<T>(this InputActionReference inputActionReference) where T : struct {
            return inputActionReference.action.ReadValue<T>();
        }

        ///<summary>
        ///Calls the function in its action.
        ///</summary>
        public static bool WasPressedThisFrame(this InputActionReference inputActionReference) {
            return inputActionReference.action.WasPressedThisFrame();
        }

        ///<summary>
        ///Calls the function in its action.
        ///</summary>
        public static bool WasReleasedThisFrame(this InputActionReference inputActionReference) {
            return inputActionReference.action.WasReleasedThisFrame();
        }

        ///<summary>
        ///Calls the function in its action.
        ///</summary>
        public static bool IsPressed(this InputActionReference inputActionReference) {
            return inputActionReference.action.IsPressed();
        }

        ///<summary>
        ///Calls the function in its action.
        ///</summary>
        public static bool Triggered(this InputActionReference inputActionReference) {
            return inputActionReference.action.triggered;
        }

    }

}


