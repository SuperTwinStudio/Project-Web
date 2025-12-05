using UnityEngine;

public class TestingPotato : MonoBehaviour {
    
    [SerializeField] private int FPS = 30;

    private void Start() {
        Application.targetFrameRate = FPS;
    }

}
