using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_Lifetime;

    // Update is called once per frame
    void Update()
    {
        m_Lifetime -= Time.deltaTime;

        if(m_Lifetime <= 0) Destroy(gameObject);

        transform.position += transform.forward * m_Speed* Time.deltaTime;
    }
}
