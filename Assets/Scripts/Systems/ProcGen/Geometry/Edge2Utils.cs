using System.Collections.Generic;
using UnityEngine;

public static class Edge2Utils
{
    public static void CalculateBoundEdges(Bounds bounds, ICollection<Edge2> edges)
    {
        Vector2 extents = bounds.extents.ToVec2XZ();
        Vector2 bottomLeftCorner = bounds.center.ToVec2XZ() + new Vector2(-extents.x, -extents.y);
        Vector2 bottomRightCorner = bounds.center.ToVec2XZ() + new Vector2(extents.x, -extents.y);
        Vector2 topLeftCorner = bounds.center.ToVec2XZ() + new Vector2(-extents.x, extents.y);
        Vector2 topRightCorner = bounds.center.ToVec2XZ() + new Vector2(extents.x, extents.y);

        Edge2 rightEdge = new Edge2(topRightCorner, bottomRightCorner);
        Edge2 bottomEdge = new Edge2(bottomRightCorner, bottomLeftCorner);
        Edge2 leftEdge = new Edge2(bottomLeftCorner, topLeftCorner);
        Edge2 topEdge = new Edge2(topLeftCorner, topRightCorner);

        edges.Add(rightEdge);
        edges.Add(bottomEdge);
        edges.Add(leftEdge);
        edges.Add(topEdge);
    }
}