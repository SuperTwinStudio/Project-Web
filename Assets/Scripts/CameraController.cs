using UnityEngine;

public class CameraController : MonoBehaviour {

    //Level
    [Header("Level")]
    [SerializeField] private Level _level;

    public Level Level => _level;

    //Components
    [Header("Components")]
    [SerializeField] private Camera _camera;

    private Transform cameraTransform;

    public Transform Follow { get; set; }
    public Camera Camera => _camera;


    //State
    private void Start() {
        cameraTransform = transform;    
    }

    private void LateUpdate() {
        cameraTransform.position = Follow.position;
    }

}
