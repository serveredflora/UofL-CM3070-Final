using UnityEngine;

public static class BoundsExtensions
{
    public static float GetBottom(this Bounds bounds) => bounds.center.y - bounds.size.y * 0.5f;
    public static float GetTop(this Bounds bounds) => bounds.center.y + bounds.size.y * 0.5f;

    public static void ExpandXZ(this ref Bounds bounds, float xz) => bounds.ExpandXZ(new Vector2(xz, xz));

    public static void ExpandXZ(this ref Bounds bounds, Vector2 xz)
    {
        bounds.size = (bounds.size.ToVec2XZ() + xz).ToVec3XZ(bounds.size.y);
    }

    public static BoundsCornerResult GetCorners(this Bounds bounds)
    {
        var leftBottomFront = bounds.center - bounds.extents;
        var rightTopBack = bounds.center + bounds.extents;

        return new BoundsCornerResult()
        {
            LeftBottomFront = leftBottomFront,
            RightBottomFront = new Vector3(rightTopBack.x, leftBottomFront.y, leftBottomFront.z),
            LeftTopFront = new Vector3(leftBottomFront.x, rightTopBack.y, leftBottomFront.z),
            RightTopFront = new Vector3(rightTopBack.x, rightTopBack.y, leftBottomFront.z),
            LeftBottomBack = new Vector3(leftBottomFront.x, leftBottomFront.y, rightTopBack.z),
            RightBottomBack = new Vector3(rightTopBack.x, leftBottomFront.y, rightTopBack.z),
            LeftTopBack = new Vector3(leftBottomFront.x, rightTopBack.y, rightTopBack.z),
            RightTopBack = rightTopBack,
        };
    }
}
