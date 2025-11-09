using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GizmoVisualizer : MonoBehaviour
{

    private static GameObject gizmosVisualizer;

    private static List<(Vector3, float, Color)> sphereGizmos = new List<(Vector3, float, Color)>();


    private static void CreateVisualizer()
    {
        GameObject visualizer = new GameObject("Gizmos Visualizer");
        visualizer.AddComponent<GizmoVisualizer>();
        gizmosVisualizer = visualizer;
    }


    public static void DrawSphereGizmo(Vector3 position, float radius, Color color)
    {

        if (gizmosVisualizer == null)
        {
            CreateVisualizer();
        }
        // sphereGizmos.Add((position, radius, color));

    }


    void OnDrawGizmos()
    {
        // foreach (var sphere in sphereGizmos)
        // {
        //     DrawSphereGizmo(sphere.Item1, sphere.Item2, sphere.Item3);
        // }

        for (int i = 0; i < sphereGizmos.Count; i++)
        {
            DrawSphereGizmo(sphereGizmos[i].Item1, sphereGizmos[i].Item2, sphereGizmos[i].Item3);
        }

        sphereGizmos = new List<(Vector3, float, Color)>();
    }

}
