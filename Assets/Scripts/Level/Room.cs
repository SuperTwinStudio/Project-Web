using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Doors;
    [SerializeField, Range(0, 1)] private int m_ShowActiveDoors;

    public void InitializeDoors(int flags) 
    {
        for (int i = 0; i < 4; i++)
        {
            m_Doors[i].SetActive(((flags >> i) & 0b1) == m_ShowActiveDoors);
        }
    }
}
