using UnityEngine;

namespace Botpa {

    public class Ease {
        
        ///<summary>
        ///Curve with cubic easing at the start.
        ///</summary>
        public static float InCubic(float x) {
            return Mathf.Pow(x, 2);
        }

        ///<summary>
        ///Curve with cubic easing at the end.
        ///</summary>
        public static float OutCubic(float x) {
            return 1 - Mathf.Pow(1 - x, 2);
        }

        ///<summary>
        ///Curve with cubic easing at the start and the end.
        ///</summary>
        public static float InOutCubic(float x) {
            return x < 0.5 ? 2 * Mathf.Pow(x, 2) : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
        }

        ///<summary>
        ///Curve with exponential easing at the start.
        ///</summary>
        public static float InExpo(float x) {
            return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
        }

        ///<summary>
        ///Curve with exponential easing at the end.
        ///</summary>
        public static float OutExpo(float x) {
            return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
        }

        ///<summary>
        ///Curve with exponential easing at the start and the end.
        ///</summary>
        public static float InOutExpo(float x) {
            return x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
        }

        ///<summary>
        ///Cliff-like curve that starts and ends at 0, peaking at 1 on x = 0.5
        ///</summary>
        public static float Cliff(float x) {
            return 1 - Mathf.Pow(2 * (x - 0.5f), 2);
        }

        ///<summary>
        ///Curve with bounce like easing at the start.
        ///</summary>
        public static float InBounce(float x) {
            return 1 - OutBounce(1 - x);
        }

        ///<summary>
        ///Curve with bounce like easing at the end.
        ///</summary>
        public static float OutBounce(float x) {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            if (x < 1f / d1)
                return n1 * x * x;
            else if (x < 2 / d1)
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            else if (x < 2.5 / d1)
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            else
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }

}