using System.Drawing;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Doors;
    [SerializeField, Range(0, 1)] private int m_ShowActiveDoors;
    [Space]
    [SerializeField] private GameObject[] m_DoorSigns;
    [SerializeField] private bool m_ShowDoorSigns;

    private Transform m_GeometryTransform = null;

    private bool m_FadeTrigger = false;
    private bool m_Faded = true;
    private float m_Size = 1.0f;

    private void Update()
    {
        if (m_Faded) return;

        if (m_FadeTrigger)
        {
            if (m_Size > 0)
            {
                m_Size -= 2 * Time.deltaTime;

                m_GeometryTransform.localScale = Vector3.one * m_Size;
            }
            else
            {
                m_GeometryTransform.gameObject.SetActive(false);
                m_Faded = true;
            }
        }
        else
        {
            if(m_Size < 1)
            {
                m_Size += 2 * Time.deltaTime;
                m_GeometryTransform.localScale = Vector3.one * m_Size;
            }
            else
            {
                m_Faded = true;
            }
        }
    }

    public void InitializeDoors(int flags)
    {
        m_GeometryTransform = transform.GetChild(0);

        for (int i = 0; i < 4; i++)
        {
            m_Doors[i].SetActive(((flags >> i) & 0b1) == m_ShowActiveDoors);

            if (m_ShowDoorSigns)
            {
                m_DoorSigns[i].SetActive(((flags >> i) & 0b1) == 1);
            }
        }

        // Shrink all rooms but the starting room
        if (((flags >> (int)RoomFlags.IS_START) & 0b1) == 0) 
        {
            m_Size = 0;
            m_GeometryTransform.localScale = Vector3.one * m_Size;
        }
    }

    public void FadeIn()
    {
        Debug.Log("Fade in");

        m_GeometryTransform.gameObject.SetActive(true);
        m_Faded = false;
        m_FadeTrigger = false;
    }

    public void FadeOut()
    {
        Debug.Log("Fade out");

        m_Faded = false;
        m_FadeTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeOut();
        }
    }
}
