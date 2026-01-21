using DG.Tweening;
using UnityEngine;


public static class TransformExtension
{
    /**
         *Get direction from this to target object;
         */
    public static Vector3 GetDirection(this Transform transform, Vector3 target)
    {
        return (target - transform.position).normalized;
    }

    public static void RotateZ(this Transform transform, float value)
    {
        var currentAngle = transform.eulerAngles;
        transform.localRotation = Quaternion.Euler(currentAngle.x, currentAngle.y, value);
    }

    public static Vector2 GetDirection(this Transform transform, Vector2 target)
    {
        return (target - (Vector2)transform.position).normalized;
    }

    public static float GetMagnitude(this Transform transform, Vector3 target)
    {
        return (target - transform.position).sqrMagnitude;
    }
}