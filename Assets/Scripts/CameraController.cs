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
    [SerializeField] private Volume postproVolume;
    [SerializeField] private Transform focusTarget;
    [SerializeField] private Shader transparentShader;

    private Transform controllerTransform, cameraTransform;
    private DepthOfField DOF;

    public Transform Follow { get; set; }

    public Camera Camera => _camera;

    //State
    private void Start() {
        //Get transforms
        controllerTransform = transform;
        cameraTransform = Camera.transform;

        //Get depth of field
        postproVolume.profile.TryGet(out DOF);
    }

    private void LateUpdate()
    {
        //Move to follow
        controllerTransform.position = Follow.position;

        //Update DOF
        float distance = Vector3.Distance(cameraTransform.position, focusTarget.position);
        DOF.focusDistance.value = distance;

        Shader.SetGlobalFloat("_PlayerDistance", distance);
    }

}
