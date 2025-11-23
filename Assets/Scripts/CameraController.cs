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
    [SerializeField] private Volume postproVolume;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Transform playerPositionTarget;
    [SerializeField] private Transform playerViewTarget;

    private Transform controllerTransform, playerTransform;
    private DepthOfField DOF;

    public Camera Camera => _camera;

    //Travel
    [Header("Travel")]
    [SerializeField] private float cameraSpeed = 100;

    private Transform moveTo, lookAt;
    private Coroutine travelCoroutine = null;

    public bool IsOnCutscene { get; private set; } = false;

    //Knockback
    [Header("Knockback")]
    [SerializeField] private bool knockbackEnabled = true;
    [SerializeField] private float knockbackMultiplier = 1;
    [SerializeField] private float knockbackDeceleration = 2;

    private Vector3 knockbackDirection = Vector3.zero;
    private float currentKnockbackDeceleration = 0;

    private readonly List<KnockbackController> knockbacks = new();
    private readonly List<Botpa.Timer> shakes = new();


    //State
    private void Start() {
        //Get transforms
        controllerTransform = transform;
        playerTransform = Level.Player.transform;

        //Get depth of field
        postproVolume.profile.TryGet(out DOF);

        //Exit cutscene on start
        ExitCutscene();
    }

    private void LateUpdate() {
        //Update shake
        for (int i = shakes.Count - 1; i >= 0; i--) {
            if (shakes[i].IsFinished) {
                shakes.RemoveAt(i);
            }
        }
        cameraAnimator.SetBool("IsShaking", !shakes.IsEmpty());

        //Apply knockback acceleration to direction
        float currentKnockbackDistance = Mathf.Max(knockbackDirection.magnitude, 1);
        for (int i = knockbacks.Count - 1; i >= 0; i--) {
            //Get knockback
            var knockback = knockbacks[i];

            //Add direction
            knockbackDirection += knockback.Strength * knockbackMultiplier / currentKnockbackDistance * Time.deltaTime / knockback.Duration * knockback.Direction;

            //Check if knockback acceleration finished
            if (knockback.Finished) knockbacks.RemoveAt(i);
        }

        //Decelerate knockback
        if (!knockbackDirection.IsEmpty() && knockbacks.IsEmpty()) {
            currentKnockbackDeceleration += Time.deltaTime * knockbackMultiplier * knockbackDeceleration;
            knockbackDirection = Vector3.MoveTowards(knockbackDirection, Vector3.zero, Time.deltaTime * currentKnockbackDeceleration);
        }

        //Move controller to player position applying knockback
        controllerTransform.position = playerTransform.position + knockbackDirection;

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
        IsOnCutscene = false;
    }

    //Camera knockback
    private class KnockbackController {

        private readonly Botpa.Timer timer = new();

        public Vector3 Direction { get; private set; }
        public float Strength { get; private set; }

        public float Duration => timer.Duration;
        public bool Finished => timer.IsFinished;

        public KnockbackController(Vector3 direction, float strength, float duration) {
            //Save values
            Direction = direction;
            Strength = Mathf.Max(strength, 0);

            //Start knockback duration count
            timer.Count(duration);
        }

    }

    public void AddKnockback(Vector3 direction, float strength = 1f, float duration = 0.05f) {
        //Ignore knockback
        if (!knockbackEnabled) return;

        //Add knockback
        knockbacks.Add(new(direction, strength, duration));

        //Reset deceleration
        currentKnockbackDeceleration = 0;
    }

    public void AddShake(float duration = 0.4f) {
        //Ignore knockback
        if (!knockbackEnabled) return;

        //Add shake
        shakes.Add(new(duration));

        //Start shake animation
        cameraAnimator.SetBool("IsShaking", true);
    }

}
