using System.Collections;
using Botpa;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    //State
    private void Start() {

        //Get transforms
        controllerTransform = transform;
        cameraTransform = Camera.transform;

        //Get depth of field
        postproVolume.profile.TryGet(out DOF);

        //Update follow
        UpdateFollow(positionTarget, viewTarget);
    }

    private void LateUpdate()
    {

        controllerTransform.position = playerTransform.position;


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
        float time = Vector3.Distance(follow.position, cameraTransform.position) / cameraSpeed;
        StartCoroutine(Traveling(time, cameraTransform.position, cameraTransform.rotation));
        onCutScene = true;
    }

    public void ExitCutScene()
    {
        UpdateFollow(positionTarget, viewTarget);
        float time = Vector3.Distance(follow.position, cameraTransform.position) / cameraSpeed;
        StartCoroutine(Traveling(time, cameraTransform.position, cameraTransform.rotation));
        onCutScene = false;
    }

    private IEnumerator Traveling(float time, Vector3 ogPosition, Quaternion ogRotation, float percent = 0)
    {
        Vector3 move = follow.position - ogPosition;
        percent += Time.deltaTime / time;
        cameraTransform.position = ogPosition + (Ease.InOutCubic(percent) * move);
        cameraTransform.rotation = Quaternion.Lerp(ogRotation, follow.rotation, percent);

        yield return new WaitForNextFrameUnit();

        if (percent > 1)
        {
            cameraTransform.position = follow.position;
        }
        else
        {
            
            StartCoroutine(Traveling(time, ogPosition, ogRotation, percent));
        }
    }
}
