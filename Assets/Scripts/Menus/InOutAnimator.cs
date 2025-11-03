using UnityEngine;

public class InOutAnimator : MonoBehaviour {

    //InOut
    [Header("InOut")]
    [SerializeField] private Material material;
    [SerializeField] private float percent = 0;

    private float percentReal = 0;


    //State
    private void LateUpdate() {
        //Not changed
        if (percentReal == percent) return;

        //Update material
        percentReal = percent;
        material.SetFloat("_Percent", Mathf.Clamp01(percentReal));
    }

    private void OnDestroy() {
        //Reset material
        material.SetFloat("_Percent", 0);
    }

}
