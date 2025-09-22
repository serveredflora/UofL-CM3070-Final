using System;
using System.Collections.Generic;
using System.Linq;

public class RoadNetwork : IDisposable
{
    public float Thickness = 2.0f;

    public IReadOnlyList<RoadNode> Nodes => _Nodes;
    public IReadOnlyList<RoadFace> Faces => _Faces;
    public IReadOnlyList<RoadSegment> Segments => _Segments;

    private List<RoadNode> _Nodes = new();
    private List<RoadFace> _Faces = new();
    private List<RoadSegment> _Segments = new();

    // TODO: cache this?
    public IEnumerable<RoadEdge> Edges
    {
        get
        {
            HashSet<RoadNode> processedNodes = new();

            foreach (var node in _Nodes)
            {
                foreach (var otherNode in node.ConnectedNodes)
                {
                    if (processedNodes.Contains(otherNode))
                    {
                        continue;
                    }

                    yield return new RoadEdge(node, otherNode);
                }

                processedNodes.Add(node);
            }
        }
    }

    public void AddNode(RoadNode node)
    {
        _Nodes.Add(node);
    }

    public void AddFace(RoadFace face)
    {
        _Faces.Add(face);
    }

    public void AddSegment(RoadSegment segment)
    {
        _Segments.Add(segment);
    }

    public void Dispose()
    {
        _Segments.Dispose();
    }
}