using System.Collections.Generic;
using Botpa;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AttackAreaIndicator : MonoBehaviour {

    //AOE
    [Header("AOE")]
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private int segments = 25;

    private MeshRenderer meshRenderer;
    private Mesh mesh;
    private readonly Timer durationTimer = new();


    //State
    private void Awake() {
        //Get mesh renderer
        meshRenderer = GetComponent<MeshRenderer>();

        //Start disappearing
        durationTimer.Count(duration);
    }

    private void Update() {
        //Update material
        meshRenderer.material.SetFloat("_Percent", 1 - durationTimer.Percent);

        //Check if finished
        if (durationTimer.IsFinished) Destroy(gameObject);
    }

    //Generation
    public void GenerateIndicator(float r, float l, bool inFront) {
        //Generate mesh
        if (!mesh) {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        //Clear mesh
        mesh.Clear();

        //Generate mesh
        GenerateCapsuleMesh(r, l, inFront);
    }

    private void GenerateCapsuleMesh(float r, float l, bool inFront) {
        //Fix 0 length
        if (l < 0.01f) l = 0.01f; 

        //Calculate the Spine Positions
        Vector3 startPos = inFront ? Vector3.forward * r : Vector3.zero;       // The center of the back circle
        Vector3 endPos = inFront ? Vector3.forward * (r + l) : Vector3.zero;   // The center of the front circle

        //Temp lists
        List<Vector3> verts = new();
        List<int> tris = new();
        List<Vector2> uvList = new();

        //Add the cap centers (Indices 0 and 1)
        verts.Add(startPos); //Index 0
        verts.Add(endPos);   //Index 1
        uvList.Add(new Vector2(0.5f, 0.5f));
        uvList.Add(new Vector2(0.5f, 0.5f));

        //Create front cap
        int frontCapStartIndex = verts.Count;
        int segmentsPerCap = segments; // Smoothness

        for (int i = 0; i <= segmentsPerCap; i++) {
            //Interpolate from 0 to 180 degrees
            float percent = (float) i / segmentsPerCap;
            float angle = Mathf.Lerp(0, 180, percent); 
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * r;
            float z = Mathf.Sin(rad) * r;

            verts.Add(new Vector3(x, 0, z) + endPos);
            uvList.Add(new Vector2(1, 1)); // Edge UV
        }

        for (int i = 0; i < segmentsPerCap; i++) {
            tris.Add(1); //Center End
            tris.Add(frontCapStartIndex + i + 1);
            tris.Add(frontCapStartIndex + i);
        }

        //Create back cap
        int backCapStartIndex = verts.Count;

        for (int i = 0; i <= segmentsPerCap; i++) {
            //Interpolate from 180 to 360 degrees
            float percent = (float) i / segmentsPerCap;
            float angle = Mathf.Lerp(180, 360, percent);
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * r;
            float z = Mathf.Sin(rad) * r;

            verts.Add(new Vector3(x, 0, z) + startPos);
            uvList.Add(new Vector2(1, 1));
        }

        for (int i = 0; i < segmentsPerCap; i++) {
            tris.Add(0); //Center Start
            tris.Add(backCapStartIndex + i + 1);
            tris.Add(backCapStartIndex + i);
        }

        //Bridge
        int frontRight = frontCapStartIndex;
        int frontLeft = frontCapStartIndex + segmentsPerCap;
        int backLeft = backCapStartIndex;
        int backRight = backCapStartIndex + segmentsPerCap;

        //Front-Left to Back-Left
        tris.Add(1);         //End Center
        tris.Add(backLeft);  //Back-Left edge
        tris.Add(frontLeft); //Front-Left edge

        tris.Add(1);         //End Center
        tris.Add(0);         //Start Center
        tris.Add(backLeft);  //Back-Left edge

        //Front-Right to Back-Right
        tris.Add(0);          //Start Center
        tris.Add(1);          //End Center
        tris.Add(frontRight); //Front-Right edge

        tris.Add(0);          //Start Center
        tris.Add(frontRight); //Front-Right edge
        tris.Add(backRight);  //Back-Right edge

        //Apply to mesh
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

}