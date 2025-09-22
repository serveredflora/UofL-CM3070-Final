using UnityEngine;

public static class Vector2Extensions
{
    // Swizzling
    public static Vector3 ToVec3XY(this Vector2 vec, float z = 0.0f) => new Vector3(vec.x, vec.y, z);
    public static Vector3 ToVec3XZ(this Vector2 vec, float y = 0.0f) => new Vector3(vec.x, y, vec.y);

    // Math operations
    public static Vector2 RotateByDegrees(this Vector2 v, float degrees) => RotateByRadians(v, degrees * Mathf.Deg2Rad);

    public static Vector2 RotateByRadians(this Vector2 v, float radians)
    {
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float cachedX = v.x;
        v.x = (cos * cachedX) - (sin * v.y);
        v.y = (sin * cachedX) + (cos * v.y);

        return v;
    }

    public static float GetSignedAngle(this Vector2 v)
    {
        // Note: the Unity method returns a counter-clockwise angle, so the args are reversed to get a clockwise one
        return Vector2.SignedAngle(v, Vector2.right);
    }
}