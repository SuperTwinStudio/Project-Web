using UnityEngine;

public class DebugRegen : MonoBehaviour
{
    [SerializeField] private LevelGenerator m_Generator;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R)) m_Generator.GenerateDefaultLevel();
    }
}
