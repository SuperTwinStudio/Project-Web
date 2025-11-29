using UnityEngine;
using UnityEngine.AI;

public class RootMotionNavMesh : MonoBehaviour {
        
    public bool ApplyRootmotion = true;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator animator;

    private Transform t;

    private void Start() {
        t = navMeshAgent.gameObject.transform;
    }

    private void OnAnimatorMove() {
        if (ApplyRootmotion) Move();
    }

    private void Move() {
        if (!navMeshAgent.isOnNavMesh) return;
        Vector3 newPosition = t.position + animator.deltaPosition;
        Vector3 vel = newPosition - t.position;
        navMeshAgent.Move(vel);
    }
    
}
