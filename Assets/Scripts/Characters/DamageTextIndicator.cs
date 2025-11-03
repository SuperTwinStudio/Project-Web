using Botpa;
using TMPro;
using UnityEngine;

public class DamageTextIndicator : MonoBehaviour {

    //Components
    [Header("Components")]
    [SerializeField] private TMP_Text text;

    //Animation
    [Header("Animation")]
    [SerializeField] private float duration = 2;
    [SerializeField, Min(0)] private float coneAngle = 15;
    [SerializeField] private float endScale = 2;
    [SerializeField] private float endDistance = 1;

    private Vector3 spawn, moveDirection;
    private readonly Timer animationTimer = new();


    //State
    private void Start() {
        //Save spawn
        spawn = transform.position;

        //Create move direction
        moveDirection = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * Quaternion.AngleAxis(Random.Range(0f, coneAngle), Vector3.right) * Vector3.up;

        //Start animation timer
        animationTimer.Count(duration);
    }

    private void Update() {
        //Finish
        if (animationTimer.finished) {
            Destroy(gameObject);
            return;
        }

        //Animate
        float percent = Ease.OutCubic(animationTimer.percent);

        transform.localScale = Util.Vec3(Mathf.Lerp(1, endScale, percent));

        transform.position = spawn + percent * endDistance * moveDirection;

        Color color = text.color;
        color.a = 1 - percent;
        text.color = color;
    }

    private void LateUpdate() {
        //Look towards camera
        transform.rotation = Quaternion.LookRotation(transform.position - Game.Current.Level.CameraController.Camera.transform.position, Vector3.up);
    }

}
