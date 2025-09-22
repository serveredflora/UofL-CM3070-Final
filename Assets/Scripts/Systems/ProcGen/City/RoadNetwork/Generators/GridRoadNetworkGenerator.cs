using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProcGen/RoadNetwork/Grid", fileName = "GridRoadNetworkGenerator")]
public class GridRoadNetworkGenerator : ARoadNetworkGenerator
{
    [Header("Settings")]
    public float RoadThickness = 8.0f;
    public float RoadSegmentThickness = 0.25f;
    public Vector2Int GridCells = new Vector2Int(6, 6);
    public Vector2 CellDimensions = new Vector2(6.5f, 8.5f);

    [Header("References")]
    public GameObject RoadSegmentPrefab;

    private Dictionary<Vector2Int, RoadNode> TempRoadNodes = new();

    public override RoadNetwork GenerateRoadNetwork(ProcGenGeneratorUtility generatorUtility)
    {
        var categoryGameObj = generatorUtility.RequestCategoryGameObject("RoadNetwork");

        var roadNetwork = new RoadNetwork();
        roadNetwork.Thickness = RoadThickness;

        for (int cellY = 0; cellY < GridCells.y; ++cellY)
        {
            for (int cellX = 0; cellX < GridCells.x; ++cellX)
            {
                var bottomLeftNode = GetOrCreateNode(roadNetwork, cellX, cellY);
                var bottomRightNode = GetOrCreateNode(roadNetwork, cellX + 1, cellY);
                var topLeftNode = GetOrCreateNode(roadNetwork, cellX, cellY + 1);

                bottomLeftNode.AddConnectedNode(bottomRightNode);
                bottomLeftNode.AddConnectedNode(topLeftNode);

                var topRightNode = GetOrCreateNode(roadNetwork, cellX + 1, cellY + 1);

                var face = new RoadFace();
                face.Points.Add(bottomLeftNode.Point);
                face.Points.Add(topLeftNode.Point);
                face.Points.Add(topRightNode.Point);
                face.Points.Add(bottomRightNode.Point);

                roadNetwork.AddFace(face);

                if (cellX == GridCells.x - 1)
                {
                    bottomRightNode.AddConnectedNode(topRightNode);
                }
                if (cellY == GridCells.y - 1)
                {
                    topLeftNode.AddConnectedNode(topRightNode);
                }
            }
        }

        TempRoadNodes.Clear();

        // TODO: ...
        float HeightOffset = -0.6f;

        foreach (var roadNode in roadNetwork.Nodes)
        {
            var roadSegment = new RoadSegment();
            roadNetwork.AddSegment(roadSegment);

            var position = roadNode.Point;
            var rotation = Quaternion.identity;
            var scale = new Vector3(roadNetwork.Thickness, RoadSegmentThickness, roadNetwork.Thickness);

            var roadSegmentGameObj = GameObject.Instantiate(RoadSegmentPrefab, position, rotation, categoryGameObj.transform);
            roadSegment.GameObjectInstance = roadSegmentGameObj;

            roadSegmentGameObj.transform.localScale = scale;
        }

        foreach (var roadEdge in roadNetwork.Edges)
        {
            var center = Vector3.Lerp(roadEdge.NodeA.Point, roadEdge.NodeB.Point, 0.5f);

            var diff = roadEdge.NodeB.Point - roadEdge.NodeA.Point;
            diff.x = Mathf.Abs(diff.x);
            diff.y = Mathf.Abs(diff.y);
            diff.z = Mathf.Abs(diff.z);

            var isXAligned = diff.x > diff.z;

            var roadSegment = new RoadSegment();
            roadNetwork.AddSegment(roadSegment);

            var position = center;
            var rotation = Quaternion.identity;
            var scale = new Vector3(isXAligned ? diff.x - roadNetwork.Thickness : roadNetwork.Thickness, RoadSegmentThickness, isXAligned ? roadNetwork.Thickness : diff.z - roadNetwork.Thickness);

            var roadSegmentGameObj = GameObject.Instantiate(RoadSegmentPrefab, position, rotation, categoryGameObj.transform);
            roadSegment.GameObjectInstance = roadSegmentGameObj;

            roadSegmentGameObj.transform.localScale = scale;
        }

        return roadNetwork;
    }

    private RoadNode GetOrCreateNode(RoadNetwork roadNetwork, int cellX, int cellY)
    {
        if (TempRoadNodes.TryGetValue(new Vector2Int(cellX, cellY), out RoadNode node))
        {
            return node;
        }

        float originX = -GridCells.x * CellDimensions.x * 0.5f;
        float originY = -GridCells.y * CellDimensions.y * 0.5f;

        var point = new Vector3(originX + CellDimensions.x * cellX, 0.0f, originY + CellDimensions.y * cellY);
        node = new RoadNode(point);

        roadNetwork.AddNode(node);

        return node;
    }
}
