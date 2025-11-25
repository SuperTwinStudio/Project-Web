using UnityEngine;

public class PlatformHide : MonoBehaviour
{
    [SerializeField] private RuntimePlatform m_ExcludedPlatform;

    void Start()
    {
        gameObject.SetActive(Application.platform != m_ExcludedPlatform);
    }
}
