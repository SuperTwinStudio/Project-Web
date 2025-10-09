using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour {

    //Level
    [Header("Level")]
    [SerializeField] private Level _level;

    public Level Level => _level;

    //Components
    [Header("Components")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform focusTarget;
    [SerializeField] private Volume postproVolume;
    

    private Transform cameraTransform;

    public Transform Follow { get; set; }
    public Camera Camera => _camera;

    private DepthOfField DOF;


    //State
    private void Start() {
        cameraTransform = transform;    
        if (postproVolume.profile.TryGet(out DepthOfField _DOF))
        {
            DOF = _DOF;
        }
    }

    private void LateUpdate() {
        cameraTransform.position = Follow.position;
        DOF.focusDistance.value = Vector3.Distance(cameraTransform.position, focusTarget.position);
    }

}
