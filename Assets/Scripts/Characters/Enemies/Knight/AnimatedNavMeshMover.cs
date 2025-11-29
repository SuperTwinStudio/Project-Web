using UnityEngine;
using UnityEngine.AI;

public class AnimatedNavMeshMover : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private Transform animatedModelTransform;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private void Start() {
        //Agent shouldn't move itself
        navMeshAgent.autoTraverseOffMeshLink = false;
        navMeshAgent.autoBraking = false;
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
    }

    private void Update() {
        navMeshAgent.nextPosition = navMeshAgent.transform.position + animator.deltaPosition;
        
        /*if (animatedModelTransform != null && navMeshAgent.enabled) {
            // 🌸 Step 1: Tell the Agent where it *should* be going 🌸
            // We set the velocity based on the movement of the animated object
            Vector3 worldDeltaPosition = animatedModelTransform.position - navMeshAgent.nextPosition;
            navMeshAgent.velocity = worldDeltaPosition / Time.deltaTime;

            // 💖 Step 2: Manually move the Agent to the Animated Model's position 💖
            // This is the magic part! The agent's position is updated to match the animated transform.
            navMeshAgent.nextPosition = animatedModelTransform.position;
        }*/
    }

}