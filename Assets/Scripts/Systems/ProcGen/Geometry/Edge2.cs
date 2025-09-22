using UnityEngine;
using System.Diagnostics.Contracts;

public class Edge2
{
    public Vector2 PointA;
    public Vector2 PointB;

    public Vector2 Diff => PointB - PointA;
    public Vector2 Center => PointA + (Diff * 0.5f);

    public readonly float Length;
    public readonly float Angle;

    public Edge2(Vector2 pointA, Vector2 pointB)
    {
        PointA = pointA;
        PointB = pointB;

        Vector2 diff = PointB - PointA;
        Length = diff.magnitude;
        Angle = diff.normalized.GetSignedAngle();
    }

    [Pure]
    public Vector2 Lerp(float t)
    {
        return Vector2.Lerp(PointA, PointB, t);
    }

    [Pure]
    public bool IsSame(Edge2 other)
    {
        const float eps = 0.01f;

        bool isSameAToOA = (other.PointA - PointA).magnitude <= eps;
        bool isSameAToOB = (other.PointB - PointA).magnitude <= eps;
        bool isSameBToOA = (other.PointA - PointB).magnitude <= eps;
        bool isSameBToOB = (other.PointB - PointB).magnitude <= eps;

        // return (isSameAToOA || isSameAToOB) && (isSameBToOA || isSameBToOB);
        return (isSameAToOA && isSameBToOB) && (isSameAToOB && isSameBToOA);
        // return ((isSameAToOA || isSameAToOB) && (isSameBToOA || isSameBToOB)) || ((isSameAToOA && isSameBToOB) && (isSameAToOB && isSameBToOA));
    }
}
