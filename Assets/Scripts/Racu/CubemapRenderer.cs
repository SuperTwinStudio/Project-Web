using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CubemapRenderer : MonoBehaviour
{
    [SerializeField] private Cubemap cubemap;
    public Camera myCamera;

    void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    public void RenderCubemap()
    {
        myCamera.RenderToCubemap(cubemap);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(CubemapRenderer))]
public class CubemapRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {

        var supper = (CubemapRenderer)target;

        if (!supper.myCamera) supper.myCamera = supper.GetComponent<Camera>();

        DrawDefaultInspector();

        if (GUILayout.Button("Render Cubemap to texture")) supper.RenderCubemap();
    }
}

#endif