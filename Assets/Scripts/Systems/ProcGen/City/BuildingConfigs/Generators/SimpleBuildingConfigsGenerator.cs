using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProcGen/BuildingConfigs/Simple", fileName = "SimpleBuildingConfigsGenerator")]
public class SimpleBuildingConfigsGenerator : ABuildingConfigsGenerator
{
    [Header("Settings")]
    public float AllotmentInset = 1.0f;

    public Vector2 BuildingHeightRange = new Vector2(8, 24);
    public Vector2 FlooringThicknessRange = new Vector2(0.25f, 1.0f);
    public Vector2 CeilingThicknessRange = new Vector2(0.25f, 1.0f);
    public Vector2 InteriorHeightRange = new Vector2(2.75f, 4.5f);
    public List<float> FloorScaleOptions = new() { 1.0f, 1.0f, 1.0f, 0.95f, 0.9f, 0.8f, 0.6f };

    public override List<BuildingConfig> GenerateBuildingConfigs(ProcGenGeneratorUtility generatorUtility, List<Allotment> allotments)
    {
        List<BuildingConfig> buildingConfigs = new();

        foreach (var allotment in allotments)
        {
            if (allotment.Type != AllotmentType.Building)
            {
                continue;
            }

            var buildingConfig = new BuildingConfig();
            buildingConfig.Bounds = allotment.Bounds;
            buildingConfig.Bounds.ExpandXZ(-AllotmentInset * 2);

            float buildingHeight = Random.Range(BuildingHeightRange.x, BuildingHeightRange.y);
            float accumulatedHeight = 0.0f;
            float accumulatedScale = 1.0f;
            while (accumulatedHeight < buildingHeight)
            {
                float flooringThickness = Random.Range(FlooringThicknessRange.x, FlooringThicknessRange.y);
                float ceilingThickness = Random.Range(CeilingThicknessRange.x, CeilingThicknessRange.y);

                float interiorHeight = Random.Range(InteriorHeightRange.x, InteriorHeightRange.y);

                float exteriorHeight = flooringThickness + ceilingThickness + interiorHeight;
                float floorStartHeight = accumulatedHeight;
                accumulatedHeight += exteriorHeight;

                float floorScale = FloorScaleOptions[Random.Range(0, FloorScaleOptions.Count)];
                accumulatedScale *= floorScale;

                var floorBounds = buildingConfig.Bounds;
                floorBounds.center = floorBounds.center.WithY(floorBounds.GetTop() + floorStartHeight + exteriorHeight * 0.5f);
                floorBounds.size = (floorBounds.size.ToVec2XZ() * accumulatedScale).ToVec3XZ(exteriorHeight);

                var floorConfig = new BuildingFloorConfig();
                floorConfig.Bounds = floorBounds;
                floorConfig.FlooringThickness = flooringThickness;
                floorConfig.CeilingThickness = ceilingThickness;
                floorConfig.InteriorHeight = interiorHeight;
                floorConfig.ExteriorHeight = exteriorHeight;

                buildingConfig.FloorConfigs.Add(floorConfig);
            }

            buildingConfig.Bounds.center = buildingConfig.Bounds.center.WithY(buildingConfig.Bounds.GetTop() + accumulatedHeight * 0.5f);
            buildingConfig.Bounds.size = buildingConfig.Bounds.size.WithY(accumulatedHeight);

            buildingConfigs.Add(buildingConfig);
        }

        return buildingConfigs;
    }
}
