using UnityEngine;

public static class VectorUtils
{
    public static void ClosestPointToLine(Vector2 l1, Vector2 l2, Vector2 point, out Vector2 intersectionPoint, out float distance)
    {
        Vector2 lineDiff = l2 - l1;
        float u = Vector2.Dot((point - l1), lineDiff) / Vector2.Dot(lineDiff, lineDiff);
        if (u < 0)
        {
            intersectionPoint = l1;
        }
        else if (u > 1)
        {
            intersectionPoint = l2;
        }
        else
        {
            intersectionPoint = l1 + u * lineDiff;
        }

        distance = Vector3.Distance(intersectionPoint, point);
    }

    // "Intersection point of two line segments in 2 dimensions": https://paulbourke.net/geometry/pointlineplane/
    public static bool LineIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersectionPoint)
    {
        float denom = (b2.y - b1.y) * (a2.x - a1.x) - (b2.x - b1.x) * (a2.y - a1.y);
        float numera = (b2.x - b1.x) * (a1.y - b1.y) - (b2.y - b1.y) * (a1.x - b1.x);
        float numerb = (a2.x - a1.x) * (a1.y - b1.y) - (a2.y - a1.y) * (a1.x - b1.x);

        /* Are the line coincident? */
        const float EPS = 0.001f;
        if (Mathf.Abs(numera) < EPS && Mathf.Abs(numerb) < EPS && Mathf.Abs(denom) < EPS)
        {
            intersectionPoint = (a1 + a2) * 0.5f;
            return true;
        }

        /* Are the line parallel */
        if (Mathf.Abs(denom) < EPS)
        {
            intersectionPoint = Vector2.zero;
            return false;
        }

        /* Is the intersection along the the segments */
        float mua = numera / denom;
        float mub = numerb / denom;
        if (mua < 0 || mua > 1 || mub < 0 || mub > 1)
        {
            intersectionPoint = Vector2.zero;
            return false;
        }

        intersectionPoint = a1 + (mua * (a2 - a1));
        return true;
    }
}