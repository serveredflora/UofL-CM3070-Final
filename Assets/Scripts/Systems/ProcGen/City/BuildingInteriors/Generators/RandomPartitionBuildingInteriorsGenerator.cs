using System.Collections.Generic;
using UnityEngine;
using TriInspector;
using System.Linq;

[CreateAssetMenu(menuName = "ProcGen/BuildingInteriors/Random", fileName = "RandomBuildingInteriorsGenerator")]
public class RandomBuildingInteriorsGenerator : ABuildingInteriorsGenerator
{
    [Header("Settings")]
    [Range(0.0f, 1.0f)]
    public float RecursiveSplitChance = 0.7f;

    [Min(1.0f)]
    public float MinRoomLength = 3.0f;
    [Min(1.0f)]
    public float MinLengthForDoor = 3.5f;
    public BuildingDoorDefinition DoorDefinition;

    [Header("References")]
    [Required]
    [AssetsOnly]
    public GameObject WallPrefab;
    [Required]
    [AssetsOnly]
    public GameObject CeilingLightPrefab;

    public override List<BuildingInterior> GenerateBuildingInteriors(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs)
    {
        var categoryGameObj = generatorUtility.RequestCategoryGameObject("Interior");

        List<BuildingInterior> buildingInteriors = new();

        for (int configIdx = 0; configIdx < buildingConfigs.Count; ++configIdx)
        {
            var buildingConfig = buildingConfigs[configIdx];

            var buildingInterior = new BuildingInterior();
            buildingInteriors.Add(buildingInterior);

            for (int floorIdx = 0; floorIdx < buildingConfig.FloorConfigs.Count; ++floorIdx)
            {
                var floorConfig = buildingConfig.FloorConfigs[floorIdx];

                var floorRooms = new List<BuildingRoom>();
                buildingInterior.RoomsByFloor.Add(floorConfig, floorRooms);

                float centerY = floorConfig.Bounds.GetBottom();

                Bounds baseBound = floorConfig.Bounds;

                List<Bounds> resultBounds = new() { baseBound };
                while (true)
                {
                    bool shouldSplit = Random.Range(0.0f, 1.0f) < RecursiveSplitChance;
                    if (!shouldSplit)
                    {
                        break;
                    }

                    // TODO: this can be optimized by keeping track of which bounds are possible based on their size's lengths
                    Bounds? selectedBound = null;
                    float splitT = 0.0f;
                    bool splitXAxis = false;
                    const int MaxAttempts = 50;
                    int attempts = 0;
                    while (attempts < MaxAttempts)
                    {
                        selectedBound = resultBounds[Random.Range(0, resultBounds.Count)];

                        splitT = Random.Range(0.0f, 1.0f);
                        splitXAxis = Random.Range(0.0f, 1.0f) < 0.5f;
                        float boundLength = splitXAxis ? selectedBound.Value.size.x : selectedBound.Value.size.z;

                        if (boundLength * splitT < MinRoomLength || boundLength * (1.0f - splitT) < MinRoomLength)
                        {
                            selectedBound = null;
                            ++attempts;
                            continue;
                        }

                        break;
                    }

                    if (!selectedBound.HasValue)
                    {
                        // Assume not possible
                        break;
                    }

                    Bounds boundsA = selectedBound.Value;
                    Bounds boundsB = selectedBound.Value;

                    if (splitXAxis)
                    {
                        float wA = splitT * selectedBound.Value.size.x;
                        float wB = (1.0f - splitT) * selectedBound.Value.size.x;

                        Vector3 bACenter = boundsA.center;
                        bACenter.x -= wB * 0.5f;
                        boundsA.center = bACenter;

                        Vector3 bBCenter = boundsB.center;
                        bBCenter.x += wA * 0.5f;
                        boundsB.center = bBCenter;

                        boundsA.size = boundsA.size.WithX(wA);
                        boundsB.size = boundsB.size.WithX(wB);
                    }
                    else
                    {
                        float wA = splitT * selectedBound.Value.size.z;
                        float wB = (1.0f - splitT) * selectedBound.Value.size.z;

                        Vector3 bACenter = boundsA.center;
                        bACenter.z -= wB * 0.5f;
                        boundsA.center = bACenter;

                        Vector3 bBCenter = boundsB.center;
                        bBCenter.z += wA * 0.5f;
                        boundsB.center = bBCenter;

                        boundsA.size = boundsA.size.WithZ(wA);
                        boundsB.size = boundsB.size.WithZ(wB);
                    }

                    resultBounds.Remove(selectedBound.Value);
                    resultBounds.Add(boundsA);
                    resultBounds.Add(boundsB);
                }

                List<BoundEdge2> allBoundEdges = new();
                Dictionary<Bounds, BuildingRoom> boundsToRoom = new();
                for (int boundIdx = 0; boundIdx < resultBounds.Count; ++boundIdx)
                {
                    Bounds bounds = resultBounds[boundIdx];

                    var room = new BuildingRoom();
                    floorRooms.Add(room);

                    room.Bounds = bounds;
                    boundsToRoom.Add(bounds, room);

                    var ceilingLightDetail = new BuildingRoomDetail();
                    room.Details.Add(ceilingLightDetail);

                    var ceilingLightPos = room.Bounds.center.WithY(centerY + floorConfig.FlooringThickness + floorConfig.InteriorHeight);
                    var ceilingLightRot = Quaternion.identity;

                    var ceilingLightGameObj = GameObject.Instantiate(CeilingLightPrefab, ceilingLightPos, ceilingLightRot, categoryGameObj.transform);
                    ceilingLightDetail.GameObjectInstance = ceilingLightGameObj;

                    BoundEdge2Utils.CalculateBoundsEdges(bounds, allBoundEdges);
                }

                List<BoundEdge2> allInteriorEdges = new();
                for (int boundEdgeIdx = 0; boundEdgeIdx < allBoundEdges.Count; ++boundEdgeIdx)
                {
                    BoundEdge2 boundEdge = allBoundEdges[boundEdgeIdx];
                    allInteriorEdges.Add(boundEdge);
                }

                float renderHeight = centerY;

                List<BoundEdge2> roomEdges = new();
                for (int interiorEdgeIdx = 0; interiorEdgeIdx < allInteriorEdges.Count; ++interiorEdgeIdx)
                {
                    BoundEdge2 boundEdge = allInteriorEdges[interiorEdgeIdx];

                    for (int otherBoundEdgeIdx = 0; otherBoundEdgeIdx < allInteriorEdges.Count; ++otherBoundEdgeIdx)
                    {
                        if (interiorEdgeIdx == otherBoundEdgeIdx)
                        {
                            continue;
                        }

                        BoundEdge2 otherBoundEdge = allInteriorEdges[otherBoundEdgeIdx];
                        if (boundEdge.BoundA.Equals(otherBoundEdge.BoundA))
                        {
                            continue;
                        }

                        if (boundEdge.IsSame(otherBoundEdge))
                        {
                            continue;
                        }

                        VectorUtils.ClosestPointToLine(boundEdge.PointA, boundEdge.PointB, otherBoundEdge.PointA, out Vector2 intersectionPointA, out float distA);
                        VectorUtils.ClosestPointToLine(boundEdge.PointA, boundEdge.PointB, otherBoundEdge.PointB, out Vector2 intersectionPointB, out float distB);
                        if (distA > 0.01f && distB > 0.01f)
                        {
                            continue;
                        }

                        var newEdge = new BoundEdge2(intersectionPointA, intersectionPointB, boundEdge.BoundA, otherBoundEdge.BoundA);
                        if (newEdge.Length < 0.01f)
                        {
                            continue;
                        }

                        // ALINE usage commented out
                        // Drawing.Draw.WireSphere(intersectionPointA.ToVec3XZ(baseBound.center.y), 0.125f, Color.cyan);
                        // Drawing.Draw.WireSphere(intersectionPointB.ToVec3XZ(baseBound.center.y), 0.125f, Color.cyan);

                        roomEdges.Add(newEdge);
                    }
                }

                List<BoundEdge2> deduplicatedRoomEdges = new();
                for (int doorwayEdgeIdx = 0; doorwayEdgeIdx < roomEdges.Count; ++doorwayEdgeIdx)
                {
                    BoundEdge2 boundEdge = roomEdges[doorwayEdgeIdx];

                    if (deduplicatedRoomEdges.Any(e => e.OtherIsSame(boundEdge)))
                    {
                        continue;
                    }

                    deduplicatedRoomEdges.Add(boundEdge);
                }

                roomEdges = deduplicatedRoomEdges;

                float interiorOriginY = renderHeight + floorConfig.FlooringThickness;

                for (int doorwayEdgeIdx = 0; doorwayEdgeIdx < roomEdges.Count; ++doorwayEdgeIdx)
                {
                    BoundEdge2 boundEdge = roomEdges[doorwayEdgeIdx];

                    BuildingWall wall;

                    float edgeLength = boundEdge.Length;
                    if (edgeLength >= MinLengthForDoor)
                    {
                        float inset = MinLengthForDoor * 0.5f;
                        float t1 = inset / edgeLength;
                        float t2 = 1.0f - t1;
                        float t = Random.Range(t1, t2);

                        wall = CityProcGenUtils.CreateWallWithDoor(WallPrefab, DoorDefinition, categoryGameObj.transform, boundEdge, interiorOriginY, t, floorConfig.InteriorHeight);
                    }
                    else
                    {
                        wall = new BuildingWall();

                        var edgeRot = Quaternion.Euler(0.0f, boundEdge.Angle, 0.0f);

                        var wallSegment = CityProcGenUtils.CreateWallSegment(WallPrefab, categoryGameObj.transform, boundEdge.Center.ToVec3XZ(interiorOriginY), edgeRot, boundEdge.Length, floorConfig.InteriorHeight);
                        wall.WallSegments.Add(wallSegment);
                    }

                    boundsToRoom[boundEdge.BoundA].Walls.Add(wall);
                    boundsToRoom[boundEdge.BoundB.Value].Walls.Add(wall);
                }
            }
        }

        return buildingInteriors;
    }
}
