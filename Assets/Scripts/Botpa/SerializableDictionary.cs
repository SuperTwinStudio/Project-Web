using System;
using System.Collections.Generic;
using UnityEngine;

namespace Botpa {

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {

        //Key value pairs
        [SerializeField] private List<TKey> keys = new();
        [SerializeField] private List<TValue> values = new();
        

        //Save the dictionary to lists
        public void OnBeforeSerialize() {
            //Clear lists
            keys.Clear();
            values.Clear();

            //Readd elements
            foreach(KeyValuePair<TKey, TValue> pair in this) {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
        
        //Load dictionary from lists
        public void OnAfterDeserialize() {
            //Clear dictionary
            Clear();

            //Check for errors
            if (keys.Count != values.Count)
                throw new Exception($"There are {keys.Count} keys and {values.Count} values after deserialization. Make sure that both key and value types are serializable.");

            //Readd pairs
            for (int i = 0; i < keys.Count; i++)
                Add(keys[i], values[i]);
        }
        
    }

}