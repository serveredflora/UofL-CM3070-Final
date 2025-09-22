using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RightAngledPolygon2D
{
    public IReadOnlyList<Vector2> Points => _Points;
    private List<Vector2> _Points = new();

    public float Height;

    public Vector2 Center
    {
        get
        {
            if (Points.Count == 0)
            {
                return Vector2.zero;
            }

            Vector2 sum = Points[0];
            for (int i = 1; i < Points.Count; ++i)
            {
                sum += Points[i];
            }

            return sum / Points.Count;
        }
    }

    public IEnumerable<(Vector2, Vector2)> Edges
    {
        get
        {
            for (var i = 1; i < Points.Count; ++i)
            {
                yield return (Points[i - 1], Points[i]);
            }

            if (Points.Count >= 3)
            {
                yield return (Points[^1], Points[0]);
            }
        }
    }

    public float EdgesCount => Points.Count switch
    {
        0 => 0,
        1 => 0,
        2 => 1,
        _ => Points.Count,
    };

    public Bounds Bounds
    {
        get
        {
            Bounds bounds = new Bounds(Points[0], Vector2.zero);
            for (int i = 1; i < Points.Count; i++)
            {
                Vector2 point = Points[i];
                bounds.Encapsulate(point);
            }

            return bounds;
        }
    }

    public RightAngledPolygon2D(float height)
    {
        Height = height;
    }

    public void AddPoint(Vector2 p)
    {
        if (Points.Count == 0)
        {
            _Points.Add(p);
        }
        else
        {
            // TODO: ensure axis aligned with last point
            _Points.Add(p);
        }
    }

    public (Vector2, Vector2) GetEdge(int edgeIndex) => GetEdge(edgeIndex, out _, out _);

    public (Vector2, Vector2) GetEdge(int edgeIndex, out int pointAIndex, out int pointBIndex)
    {
        GetPointIndexesForEdgeIndex(edgeIndex, out pointAIndex, out pointBIndex);

        Vector2 pointA = Points[pointAIndex];
        Vector2 pointB = Points[pointBIndex];

        return (pointA, pointB);
    }

    private void GetPointIndexesForEdgeIndex(int edgeIndex, out int pointAIndex, out int pointBIndex)
    {
        Debug.Assert(EdgesCount % 2 == 0);
        Debug.Assert(edgeIndex >= 0 && edgeIndex < EdgesCount);

        pointAIndex = edgeIndex;
        pointBIndex = (edgeIndex + 1) % Points.Count;
    }

    private bool IsXAxisAlignedEdge(Vector2 a, Vector2 b)
    {
        Vector2 diff = b - a;
        return MathF.Abs(diff.y) < 0.01f;
    }

    public bool TrySubdivide(int edgeIndex, float t, out RightAngledPolygon2D a, out RightAngledPolygon2D b)
    {
        Debug.Assert(Points.Count >= 4);

        var (pointA, pointB) = GetEdge(edgeIndex, out int pointAIndex, out int pointBIndex);

        Vector2 splitSource = Vector2.Lerp(pointA, pointB, t);
        bool edgeIsXAxisAligned = IsXAxisAlignedEdge(pointA, pointB);

        // Find other edge to split from
        int otherEdgeIndex = -1;
        Vector2 otherSplit = Vector2.zero;
        for (int scanEdgeIndex = 0; scanEdgeIndex < EdgesCount; ++scanEdgeIndex)
        {
            if (scanEdgeIndex == edgeIndex)
            {
                // Same edge
                continue;
            }

            bool isSameAxis = scanEdgeIndex % 2 == edgeIndex % 2;
            if (!isSameAxis)
            {
                // Not same axis
                continue;
            }

            var (scanPointA, scanPointB) = GetEdge(scanEdgeIndex);
            VectorUtils.ClosestPointToLine(scanPointA, scanPointB, splitSource, out var intersectionPoint, out _);

            Vector2 diff = intersectionPoint - splitSource;
            bool scanEdgeIsPerpendicular = edgeIsXAxisAligned ? MathF.Abs(diff.x) < 0.01f : MathF.Abs(diff.y) < 0.01f;
            if (!scanEdgeIsPerpendicular)
            {
                // Is not perpendicular
                continue;
            }

            // TODO: need to select the correct edge incase multiple get to this part...
            otherEdgeIndex = scanEdgeIndex;
            otherSplit = intersectionPoint;
            break;
        }

        if (otherEdgeIndex == -1)
        {
            a = null;
            b = null;
            return false;
        }

        // Found opposite split point on another edge, now generate the two new resulting polygons!
        a = new(Height);
        b = new(Height);

        GetPointIndexesForEdgeIndex(otherEdgeIndex, out int otherPointAIndex, out int otherPointBIndex);

        bool splitIsBeforeOtherSplit = pointAIndex < otherPointAIndex;

        int pointIndex;
        bool reachedSplit;

        int initialAIndex = splitIsBeforeOtherSplit ? pointAIndex : otherPointAIndex;
        pointIndex = initialAIndex;
        reachedSplit = false;
        while (!(reachedSplit && pointIndex == initialAIndex))
        {
            Vector2 p = Points[pointIndex];

            a.AddPoint(p);
            if (pointIndex == pointAIndex)
            {
                a.AddPoint(splitSource);
                a.AddPoint(otherSplit);

                reachedSplit = true;
                pointIndex = otherPointBIndex;
                continue;
            }
            else if (pointIndex == otherPointAIndex)
            {
                a.AddPoint(otherSplit);
                a.AddPoint(splitSource);

                reachedSplit = true;
                pointIndex = pointBIndex;
                continue;
            }

            // ++pointIndex;
            pointIndex = (pointIndex + 1) % Points.Count;
        }

        int initialBIndex = splitIsBeforeOtherSplit ? pointBIndex : otherPointBIndex;
        pointIndex = initialBIndex;
        reachedSplit = false;
        while (!(reachedSplit && pointIndex == initialBIndex))
        {
            Vector2 p = Points[pointIndex];

            b.AddPoint(p);
            if (pointIndex == pointAIndex)
            {
                b.AddPoint(splitSource);
                b.AddPoint(otherSplit);

                reachedSplit = true;
                break;
            }
            else if (pointIndex == otherPointAIndex)
            {
                b.AddPoint(otherSplit);
                b.AddPoint(splitSource);

                reachedSplit = true;
                break;
            }

            pointIndex = (pointIndex + 1) % Points.Count;
        }

        return true;
    }

    public void Draw(Color color)
    {
        int i = 0;
        foreach ((Vector2 p1, Vector2 p2) in Edges)
        {
            // ALINE usage commented out
            // Drawing.Draw.Label2D(p1.ToVec3XZ(Height + 0.1f), i.ToString(), 96, color);
            // Drawing.Draw.Line(p1.ToVec3XZ(Height), p2.ToVec3XZ(Height), color);
            ++i;
        }
    }
}
