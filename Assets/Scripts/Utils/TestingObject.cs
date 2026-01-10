using UnityEngine;

public class TestingObject : MonoBehaviour {

    private void Awake() {
        #if !UNITY_EDITOR
        if (!Debug.isDebugBuild) Destroy(gameObject);
        #endif
    }

}
