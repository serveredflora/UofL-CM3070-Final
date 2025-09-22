using System.Collections.Generic;
using UnityEngine;

public static class BoundEdge2Utils
{
    public static void CalculateBoundsEdges(Bounds bounds, ICollection<BoundEdge2> edges)
    {
        Vector2 extents = bounds.extents.ToVec2XZ();
        Vector2 bottomLeftCorner = bounds.center.ToVec2XZ() + new Vector2(-extents.x, -extents.y);
        Vector2 bottomRightCorner = bounds.center.ToVec2XZ() + new Vector2(extents.x, -extents.y);
        Vector2 topLeftCorner = bounds.center.ToVec2XZ() + new Vector2(-extents.x, extents.y);
        Vector2 topRightCorner = bounds.center.ToVec2XZ() + new Vector2(extents.x, extents.y);

        BoundEdge2 rightEdge = new BoundEdge2(topRightCorner, bottomRightCorner, bounds);
        BoundEdge2 bottomEdge = new BoundEdge2(bottomRightCorner, bottomLeftCorner, bounds);
        BoundEdge2 leftEdge = new BoundEdge2(bottomLeftCorner, topLeftCorner, bounds);
        BoundEdge2 topEdge = new BoundEdge2(topLeftCorner, topRightCorner, bounds);

        edges.Add(rightEdge);
        edges.Add(bottomEdge);
        edges.Add(leftEdge);
        edges.Add(topEdge);
    }
}