using System.Collections;
using System.Collections.Generic;
using Botpa;
using Unity.VisualScripting;
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
    [SerializeField] private Transform playerPositionTarget;
    [SerializeField] private Transform playerViewTarget;

    private Transform controllerTransform, cameraTransform, playerTransform;
    private DepthOfField DOF;

    public Camera Camera => _camera;

    //Travel
    [Header("Travel")]
    [SerializeField] private float cameraSpeed = 100;
    [SerializeField] private float cameraRotationSpeed = 1000;

    private Transform moveTo, lookAt;
    private Coroutine travelCoroutine = null;

    public bool IsOnCutscene { get; private set; } = false;

    //Knockback
    [Header("Knockback")]
    [SerializeField] private float maxKnockbackDistance = 1;

    private readonly List<KnockbackController> knockbacks = new();


    //State
    private void Start() {
        //Get transforms
        controllerTransform = transform;
        cameraTransform = Camera.transform;
        playerTransform = Level.Player.transform;

        //Get depth of field
        postproVolume.profile.TryGet(out DOF);

        //Update follow
        UpdateFollow(playerViewTarget, playerPositionTarget);
    }

    private void LateUpdate() {
        //Calculate knockback direction
        int knockbackActors = 0;
        Vector3 knockbackDirection = Vector3.zero;
        for (int i = knockbacks.Count - 1; i >= 0; i--) {
            //Get knockback
            var knockback = knockbacks[i];

            //Check if knockback finished
            if (knockback.Timer.finished) {
                knockbacks.RemoveAt(i);
                continue;
            }

            //Add direction
            knockbackActors++;
            knockbackDirection += knockback.LengthPercent * knockback.Strength * knockback.Direction;
        }
        //if (knockbackActors != 0) knockbackDirection /= knockbackActors;

        //Move controller to player position applying knockback
        controllerTransform.position = playerTransform.position + /*maxKnockbackDistance * */knockbackDirection;

        //Update DOF
        float distance = Vector3.Distance(cameraTransform.position, playerViewTarget.position);
        DOF.focusDistance.value = distance;

        //Update distance to player in shader
        Shader.SetGlobalFloat("_PlayerDistance", distance);
    }

    //Follow targets
    private void UpdateFollow(Transform newMoveTo, Transform newLookAt) {
        moveTo = newMoveTo;
        lookAt = newLookAt;
    }

    //Travel
    private float CalculateTravelDuration() {
        return Vector3.Distance(cameraTransform.position, moveTo.position) / cameraSpeed;
    }

    private IEnumerator TravelCoroutine(Quaternion ogRotation, Vector3 ogPosition, float time, float percent = 0) {
        //Update percent
        percent = Mathf.Clamp01(percent + Time.deltaTime / time);
        float easedPercent = Ease.InOutCubic(percent);

        //Move
        cameraTransform.position = Vector3.Lerp(ogPosition, moveTo.position, easedPercent);

        //Rotate (after movement as we use the new position to calculate rotation)
        cameraTransform.rotation = Quaternion.Lerp(ogRotation, Quaternion.LookRotation(lookAt.position - cameraTransform.position), easedPercent);

        //Wait a frame
        yield return new WaitForNextFrameUnit();

        //Continue travel
        if (percent < 1) travelCoroutine = StartCoroutine(TravelCoroutine(ogRotation, ogPosition, time, percent));
    }

    private void StartTravel(Transform newMoveTo, Transform newLookAt) {
        //Update follow targets
        UpdateFollow(newMoveTo, newLookAt);

        //Start travel
        if (travelCoroutine != null) StopCoroutine(travelCoroutine);
        travelCoroutine = StartCoroutine(TravelCoroutine(cameraTransform.rotation, cameraTransform.position, CalculateTravelDuration()));
    }

    public void EnterCutscene(Transform newMoveTo, Transform newLookAt) {
        StartTravel(newMoveTo, newLookAt);
        IsOnCutscene = true;
    }

    public void ExitCutscene() {
        StartTravel(playerPositionTarget, playerViewTarget);
        IsOnCutscene = true;
    }

    //Camera knockback
    private class KnockbackController {

        public Botpa.Timer Timer { get; private set; } = new();
        public Vector3 Direction { get; private set; }
        public float Strength { get; private set; }

        public float LengthPercent {
            get {
                float percent = Timer.percent;
                return Ease.OutCubic(Ease.Cliff(percent));
            }
        }

        public KnockbackController(Vector3 direction, float strength) {
            Timer.Count(2);
            Direction = direction;
            Strength = Mathf.Clamp01(strength);
        }

    }

    public void AddKnockback(Vector3 direction, float strength = 1) {
        knockbacks.Add(new(direction, strength));
    }

}
