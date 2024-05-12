using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionTransform
{
    static float Threadhold = 0.5f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        return Vector3.Dot(transform.forward.normalized, (target.position - transform.position).normalized) >= Threadhold;
    }
}
