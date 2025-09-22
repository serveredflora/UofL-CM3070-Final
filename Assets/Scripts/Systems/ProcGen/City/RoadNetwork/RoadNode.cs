using System.Collections.Generic;
using UnityEngine;

public class RoadNode
{
    public Vector3 Point;
    public List<RoadNode> ConnectedNodes = new();

    public RoadNode(Vector3 point)
    {
        Point = point;
    }

    public void AddConnectedNode(RoadNode other)
    {
        this.ConnectedNodes.Add(other);
        other.ConnectedNodes.Add(this);
    }
}
