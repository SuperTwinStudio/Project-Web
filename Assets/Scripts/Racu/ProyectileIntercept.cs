using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProyectileIntercept
{

    public static Vector3 InterceptPos(Vector3 shooterPos, Vector3 targetPos, Vector3 _targetVel, float bulletSpeed){
        float a,b,c;
        float timeDelta1, timeDelta2;

        Vector3 toTarget = targetPos - shooterPos;
        Vector3 targetVel = _targetVel;
        
        a = Vector3.Dot(targetVel, targetVel) - Mathf.Pow(bulletSpeed, 2);
        b = 2 * Vector3.Dot(targetVel, toTarget);
        c = Vector3.Dot(toTarget, toTarget);

        float sqrt = Mathf.Sqrt(math.pow(b,2) - (4 * a *c));
        if(sqrt == float.NaN) return Vector3.positiveInfinity;

        timeDelta1 = (-b + sqrt) / (2 * a);
        timeDelta2 = (-b - sqrt) / (2 * a);

        return targetPos + targetVel * (timeDelta1 > timeDelta2 ? timeDelta1 : timeDelta2);
    }

}
