using System;
using System.Collections.Generic;
using UnityEngine;

namespace Botpa {

    [Serializable]
    public class Spline {

        [HideInInspector, SerializeField] private List<Vector3> points;
        [HideInInspector, SerializeField] private List<Vector3> tangents;
        [HideInInspector, SerializeField] private float[] distances;
        [HideInInspector, SerializeField] private float[] straightDistances;
        [HideInInspector, SerializeField] private float[] normalizedDistances;

        [HideInInspector, SerializeField] private float strength;

        public bool isValid => points != null && points.Count >= 3;
        public float length => distances[^1];


        public Spline(List<Vector3> points) : this(points, -1) {}

        public Spline(List<Vector3> points, float strength) {
            
            //Less than 3 points
            if (points.Count < 3) throw new Exception("Three or more points are required to create a spline");
            

             /*$$$$$$           /$$             /$$             
            | $$__  $$         |__/            | $$             
            | $$  \ $$ /$$$$$$  /$$ /$$$$$$$  /$$$$$$   /$$$$$$$
            | $$$$$$$//$$__  $$| $$| $$__  $$|_  $$_/  /$$_____/
            | $$____/| $$  \ $$| $$| $$  \ $$  | $$   |  $$$$$$ 
            | $$     | $$  | $$| $$| $$  | $$  | $$ /$$\____  $$
            | $$     |  $$$$$$/| $$| $$  | $$  |  $$$$//$$$$$$$/
            |__/      \______/ |__/|__/  |__/   \___/ |______*/

            //Save points
            this.points = points;
            this.strength = strength;


             /*$$$$$$$                                                  /$$
            |__  $$__/                                                 | $$
               | $$  /$$$$$$  /$$$$$$$   /$$$$$$   /$$$$$$  /$$$$$$$  /$$$$$$   /$$$$$$$
               | $$ |____  $$| $$__  $$ /$$__  $$ /$$__  $$| $$__  $$|_  $$_/  /$$_____/
               | $$  /$$$$$$$| $$  \ $$| $$  \ $$| $$$$$$$$| $$  \ $$  | $$   |  $$$$$$
               | $$ /$$__  $$| $$  | $$| $$  | $$| $$_____/| $$  | $$  | $$ /$$\____  $$
               | $$|  $$$$$$$| $$  | $$|  $$$$$$$|  $$$$$$$| $$  | $$  |  $$$$//$$$$$$$/
               |__/ \_______/|__/  |__/ \____  $$ \_______/|__/  |__/   \___/ |_______/
                                        /$$  \ $$
                                       |  $$$$$$/
                                        \_____*/
            
            //Calculate point tangents
            tangents = new(points.Count);
            for (int i = 0; i < points.Count; i++) tangents.Add(Vector3.zero);
            
            //Inner points
            for (int i = 1; i < points.Count - 1; i++) {
                Vector3 fromPre = (points[i] - points[i - 1]).normalized;
                Vector3 fromNext = (points[i + 1] - points[i]).normalized;
                tangents[i] = (fromPre + fromNext).normalized;
            }
            
            //First & last points
            tangents[0] = Vector3.Reflect(-tangents[1], (points[1] - points[0]).normalized);
            tangents[^1] = -Vector3.Reflect(tangents[^2], (points[^2] - points[^1]).normalized);


             /*$$$$$$  /$$             /$$
            | $$__  $$|__/            | $$
            | $$  \ $$ /$$  /$$$$$$$ /$$$$$$    /$$$$$$  /$$$$$$$   /$$$$$$$  /$$$$$$   /$$$$$$$
            | $$  | $$| $$ /$$_____/|_  $$_/   |____  $$| $$__  $$ /$$_____/ /$$__  $$ /$$_____/
            | $$  | $$| $$|  $$$$$$   | $$      /$$$$$$$| $$  \ $$| $$      | $$$$$$$$|  $$$$$$
            | $$  | $$| $$ \____  $$  | $$ /$$ /$$__  $$| $$  | $$| $$      | $$_____/ \____  $$
            | $$$$$$$/| $$ /$$$$$$$/  |  $$$$/|  $$$$$$$| $$  | $$|  $$$$$$$|  $$$$$$$ /$$$$$$$/
            |_______/ |__/|_______/    \___/   \_______/|__/  |__/ \_______/ \_______/|______*/

            //Calculate straigth distances
            straightDistances = new float[points.Count - 1];
            for (int i = 0; i < points.Count - 1; i++) straightDistances[i] = (i == 0 ? 0 : straightDistances[i - 1]) + (points[i] - points[i + 1]).magnitude;


            //Calculate real distances
            distances = new float [points.Count - 1];
            float length = 0;
            Vector3 lastPoint = Hermite(0, 0);
            for (int i = 0; i < points.Count - 1; i++) {
                for (float t = 0.05f; t <= 1; t += 0.05f) {
                    Vector3 nextPoint = Hermite(i, t);
                    length += Vector3.Distance(lastPoint, nextPoint);
                    lastPoint = nextPoint;
                }
                distances[i] = length;
            }


            //Calculate normalized distances
            normalizedDistances = new float[points.Count - 1];
            for (int i = 0; i < points.Count - 1; i++) normalizedDistances[i] = distances[i] / distances[points.Count - 2];
        }

        //Hermite
        private Vector3 Hermite(int index, float t) {
            //Curve strength
            float curveStrength;
            if (strength == -1) {
                //Use straigth distance as strength
                curveStrength = straightDistances[index] - (index == 0 ? 0 : straightDistances[index - 1]);
            } else {
                //Use defined custom strength
                curveStrength = strength;
            }
            
            //Interpolate (hermite)
            float t2 = t * t;
            float t3 = t2 * t;
            return  (2f * t3 - 3f * t2 + 1f) * points[index] +
                    (-2f * t3 + 3f * t2) * points[index + 1] +
                    (t3 - 2f * t2 + t) * curveStrength * tangents[index] +
                    (t3 - t2) * curveStrength * tangents[index + 1];
        }

        ///<summary>
        ///Calculates the point in the spline that corresponds with the normalized distance t.
        ///</summary>
        public Vector3 CalculatePoint(float t) {
            //Normalize percent
            t = Mathf.Clamp01(t);
            
            //Find index of first point
            int index;
            for (index = 0; index < normalizedDistances.Length; index++) {
                if (t <= normalizedDistances[index]) break;
            }

            //Distance between both points (normalized)
            float distFirst = index == 0 ? 0 : normalizedDistances[index - 1];
            float distLast = normalizedDistances[index];
            float distBetween = distLast - distFirst;

            //Interpolate (hermite)
            return Hermite(index, (t - distFirst) / distBetween);
        }

        ///<summary>
        ///Draws the gizmos of the spline in world coordinates
        ///</summary>
        public void DrawGizmos() {
            //Not valid
            if (!isValid) return;

            //Draw spline
            Gizmos.color = Color.red;
            for (int i = 0; i < points.Count; i++) {
                Vector3 point = points[i];
                Gizmos.DrawLine(point, point + tangents[i] * 0.2f);
                Gizmos.DrawLine(point, point - tangents[i] * 0.2f);
            }

            Gizmos.color = Color.magenta;
            for (float t = 0; t <= 1; t += 0.005f) {
                Gizmos.DrawSphere(CalculatePoint(t), 0.01f);
            }
            
            Gizmos.color = Color.blue;
            foreach (Vector3 point in points) {
                Gizmos.DrawSphere(point, 0.05f);
            }
        }

        ///<summary>
        ///Draws the gizmos of the spline in local coordinates
        ///</summary>
        public void DrawGizmosLocal(Transform transform) {
            //Not valid
            if (!isValid) return;

            //Draw spline
            Gizmos.color = Color.red;
            for (int i = 0; i < points.Count; i++) {
                Vector3 point = transform.TransformPoint(points[i] - transform.localPosition);
                Gizmos.DrawLine(point, point + tangents[i] * 0.2f);
                Gizmos.DrawLine(point, point - tangents[i] * 0.2f);
            }

            Gizmos.color = Color.magenta;
            for (float t = 0; t <= 1; t += 0.005f) {
                Vector3 point = transform.TransformPoint(CalculatePoint(t) - transform.localPosition);
                Gizmos.DrawSphere(point, 0.01f);
            }
            
            Gizmos.color = Color.blue;
            foreach (Vector3 p in points) {
                Vector3 point = transform.TransformPoint(p - transform.localPosition);
                Gizmos.DrawSphere(point, 0.05f);
            }
        }
    }

}


