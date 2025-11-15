using UnityEngine;

public class TestingObject : MonoBehaviour {

    private void Awake() {
        #if !UNITY_EDITOR
        Destroy(gameObject);
        #endif
    }
    
}
