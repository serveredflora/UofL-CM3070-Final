using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolygonFace
{
    public List<Vector3> Points = new();

    public Vector3 Center
    {
        get
        {
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < Points.Count; ++i)
            {
                sum += Points[i];
            }

            return sum / Points.Count;
        }
    }

    public IEnumerable<(Vector3, Vector3)> Edges
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

    public Bounds Bounds
    {
        get
        {
            Bounds bounds = new Bounds(Points[0], Vector3.zero);
            for (int i = 1; i < Points.Count; i++)
            {
                Vector3 point = Points[i];
                bounds.Encapsulate(point);
            }

            return bounds;
        }
    }
}
