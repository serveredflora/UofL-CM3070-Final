using UnityEngine;

public static class Vector3Extensions
{
    // Swizzling
    public static Vector2 ToVec2XY(this Vector3 vec) => new Vector2(vec.x, vec.y);
    public static Vector2 ToVec2XZ(this Vector3 vec) => new Vector2(vec.x, vec.z);

    // ...
    public static Vector3 WithX(this Vector3 vec, float x) => new Vector3(x, vec.y, vec.z);
    public static Vector3 WithY(this Vector3 vec, float y) => new Vector3(vec.x, y, vec.z);
    public static Vector3 WithZ(this Vector3 vec, float z) => new Vector3(vec.x, vec.y, z);
}