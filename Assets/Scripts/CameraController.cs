using Botpa;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour {

    //Level
    [Header("Level")]
    [SerializeField] private Level _level;

    public Level Level => _level;

    //VAriables
    [Header("Variables")]
    [SerializeField] private float cameraSpeed = 100;
    [SerializeField] private float cameraRotationSpeed = 1000;

    //Components
    [Header("Components")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Volume postproVolume;
    [SerializeField] private Transform viewTarget;
    [SerializeField] private Transform positionTarget;

    private Transform controllerTransform, cameraTransform;
    private DepthOfField DOF;

    public Transform playerTransform { get; set; }

    private Transform follow;

    public Camera Camera => _camera;

    private bool onCutScene = false;
    private Transform sceneViewTarget;
    private Transform scenePositionTarget;

    //State
    private void Start() {

        //Get transforms
        controllerTransform = transform;
        cameraTransform = Camera.transform;

        //Get depth of field
        postproVolume.profile.TryGet(out DOF);

        //Update follow
        UpdateFollow(positionTarget, viewTarget);

        //Unparenting camera from camera controller
        Camera.gameObject.transform.parent = null;
    }

    private void LateUpdate()
    {

        controllerTransform.position = playerTransform.position;

        //Update camera positioning
        cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, follow.position, cameraSpeed * Time.deltaTime);
        cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, follow.rotation, cameraRotationSpeed * Time.deltaTime);

        //Update DOF
        float distance = Vector3.Distance(cameraTransform.position, viewTarget.position);
        DOF.focusDistance.value = distance;

        //Update distance to player in shader
        Shader.SetGlobalFloat("_PlayerDistance", distance);
    }
    
    private void UpdateFollow(Transform _posTarget, Transform _viewtarget)
    {
        follow = _posTarget;
        follow.forward = (_viewtarget.position - follow.position).normalized;
    }

    public void EnterCutScene(Transform _viewTarget, Transform _positionTarget)
    {
        UpdateFollow(_positionTarget, _viewTarget);
        cameraSpeed = 5;
        cameraRotationSpeed = 30;
        onCutScene = true;
    }

    public void ExitCutScene()
    {
        UpdateFollow(positionTarget, viewTarget);
        cameraSpeed = 100;
        cameraRotationSpeed = 1000;
        onCutScene = true;
    }
}
