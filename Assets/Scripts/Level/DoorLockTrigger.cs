using UnityEngine;

public class DoorLockTrigger : MonoBehaviour
{
    [SerializeField] private Room m_Room;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Room.TryLockDoors();
        }
    }
}
