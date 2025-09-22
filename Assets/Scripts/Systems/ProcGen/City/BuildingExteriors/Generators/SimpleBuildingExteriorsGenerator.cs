using System.Collections.Generic;
using TriInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ProcGen/BuildingExteriors/Simple", fileName = "SimpleBuildingExteriorsGenerator")]
public class SimpleBuildingExteriorsGenerator : ABuildingExteriorsGenerator
{
    [Header("Settings")]
    public float WallThickness = 0.3f;
    public BuildingDoorDefinition DoorDefinition;

    [Header("References")]
    [Required]
    [AssetsOnly]
    public GameObject FlooringPrefab;
    [Required]
    [AssetsOnly]
    public GameObject CeilingPrefab;
    [Required]
    [AssetsOnly]
    public GameObject WallPrefab;

    private List<Edge2> TempEdges = new();

    public override List<BuildingExterior> GenerateBuildingExteriors(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors)
    {
        var categoryGameObj = generatorUtility.RequestCategoryGameObject("Exterior");

        List<BuildingExterior> buildingExteriors = new();

        foreach (BuildingConfig buildingConfig in buildingConfigs)
        {
            BuildingExterior buildingExterior = new();

            for (int floorIdx = 0; floorIdx < buildingConfig.FloorConfigs.Count; floorIdx++)
            {
                BuildingFloorConfig floorConfig = buildingConfig.FloorConfigs[floorIdx];
                var floor = new BuildingFloor();
                buildingExterior.Floors.Add(floor);

                var bounds = floorConfig.Bounds;
                floor.Bounds = bounds;

                float floorBaseHeight = bounds.GetBottom();

                // Flooring
                {
                    var flooring = new BuildingFlooring();
                    floor.Flooring = flooring;

                    var flooringPos = bounds.center.WithY(floorBaseHeight);
                    var flooringScale = (bounds.size.ToVec2XZ() + (Vector2.one * WallThickness)).ToVec3XZ(floorConfig.FlooringThickness);

                    var flooringGameObj = GameObjectUtils.InstantiateTRS(FlooringPrefab, categoryGameObj.transform, flooringPos, Quaternion.identity, flooringScale);
                    flooring.GameObjectInstance = flooringGameObj;
                }

                // Ceiling
                {
                    var ceiling = new BuildingCeiling();
                    floor.Ceiling = ceiling;

                    var ceilingPos = bounds.center.WithY(floorBaseHeight + floorConfig.ExteriorHeight);
                    var ceilingScale = (bounds.size.ToVec2XZ() + (Vector2.one * WallThickness)).ToVec3XZ(floorConfig.CeilingThickness);

                    var ceilingGameObj = GameObjectUtils.InstantiateTRS(CeilingPrefab, categoryGameObj.transform, ceilingPos, Quaternion.identity, ceilingScale);
                    ceiling.GameObjectInstance = ceilingGameObj;
                }

                // Walls
                {
                    Edge2Utils.CalculateBoundEdges(bounds, TempEdges);

                    bool shouldSpawnDoor = floorIdx == 0;
                    // bool shouldSpawnDoor = false;
                    int doorIdx = shouldSpawnDoor ? Random.Range(0, TempEdges.Count) : -1;

                    float interiorOriginY = floorBaseHeight + floorConfig.FlooringThickness;

                    for (int edgeIdx = 0; edgeIdx < TempEdges.Count; ++edgeIdx)
                    {
                        var edge = TempEdges[edgeIdx];

                        BuildingWall wall;
                        if (doorIdx == edgeIdx)
                        {
                            // TODO: use interior to figure out where to place a door
                            float t = 0.5f;
                            wall = CityProcGenUtils.CreateWallWithDoor(WallPrefab, DoorDefinition, categoryGameObj.transform, edge, interiorOriginY, t, floorConfig.InteriorHeight, WallThickness);
                        }
                        else
                        {
                            wall = new BuildingWall();

                            var wallPos = edge.Lerp(0.5f).ToVec3XZ(interiorOriginY);
                            var edgeRot = Quaternion.Euler(0.0f, edge.Angle, 0.0f);

                            var wallSegment = CityProcGenUtils.CreateWallSegment(WallPrefab, categoryGameObj.transform, wallPos, edgeRot, edge.Length, floorConfig.InteriorHeight, WallThickness);
                            wall.WallSegments.Add(wallSegment);
                        }

                        floor.Walls.Add(wall);
                    }

                    TempEdges.Clear();
                }
            }

            buildingExteriors.Add(buildingExterior);
        }

        return buildingExteriors;
    }
}
