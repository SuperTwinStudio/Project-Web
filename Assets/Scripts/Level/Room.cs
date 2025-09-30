using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Doors;

    public void InitializeDoors(int flags) 
    {
        for (int i = 0; i < 4; i++)
        {
            m_Doors[i].SetActive(((flags >> i) & 0b1) == 1);
        }
    }
}
