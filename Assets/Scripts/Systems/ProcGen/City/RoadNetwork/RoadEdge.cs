public readonly struct RoadEdge
{
    public readonly RoadNode NodeA;
    public readonly RoadNode NodeB;

    public RoadEdge(RoadNode nodeA, RoadNode nodeB)
    {
        NodeA = nodeA;
        NodeB = nodeB;
    }
}