using UnityEngine;
using System.Diagnostics.Contracts;

public class BoundEdge2 : Edge2
{
    public Bounds BoundA;
    public Bounds? BoundB;

    public BoundEdge2(Vector2 pointA, Vector2 pointB, Bounds boundA, Bounds? boundB = null) : base(pointA, pointB)
    {
        BoundA = boundA;
        BoundB = boundB;
    }

    [Pure]
    public bool OtherIsSame(BoundEdge2 other)
    {
        return (BoundA.Equals(other.BoundA) && BoundB.Equals(other.BoundB)) || (BoundB.Equals(other.BoundA) && BoundA.Equals(other.BoundB));
    }
}