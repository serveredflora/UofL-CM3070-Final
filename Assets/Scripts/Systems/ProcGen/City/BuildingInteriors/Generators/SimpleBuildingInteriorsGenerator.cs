using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProcGen/BuildingInteriors/Simple", fileName = "SimpleBuildingInteriorsGenerator")]
public class SimpleBuildingInteriorsGenerator : ABuildingInteriorsGenerator
{
    [Header("Settings")]
    public float PillarThickness = 0.5f;
    public float PillarMinPadding = 1.0f;
    public float PillarWidthStep = 3.0f;
    public float PillarHeightStep = 2.0f;

    public override List<BuildingInterior> GenerateBuildingInteriors(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs)
    {
        List<BuildingInterior> buildingInteriors = new();

        for (int configIdx = 0; configIdx < buildingConfigs.Count; ++configIdx)
        {
            var buildingConfig = buildingConfigs[configIdx];

            var buildingInterior = new BuildingInterior();

            for (int floorIdx = 0; floorIdx < buildingConfig.FloorConfigs.Count; ++floorIdx)
            {
                var floorConfig = buildingConfig.FloorConfigs[floorIdx];

                float centerY = floorConfig.Bounds.center.y;

                Vector3 boundsSize = floorConfig.Bounds.size;

                Vector2Int pillarCount = new Vector2Int((int)Mathf.Floor((boundsSize.x - PillarMinPadding) / PillarWidthStep), (int)Mathf.Floor((boundsSize.z - PillarMinPadding) / PillarHeightStep));
                Vector2 pillarOrigin = new Vector2((pillarCount.x - 1) * PillarWidthStep, (pillarCount.y - 1) * PillarHeightStep);
                pillarOrigin *= -0.5f;

                // TODO: update with both new floor config setup & gameobject usage
                // const float bias = 0.001f;
                // for (int pillarY = 0; pillarY < pillarCount.y; ++pillarY)
                // {
                //     for (int pillarX = 0; pillarX < pillarCount.x; ++pillarX)
                //     {
                //         Vector3 pillarPosition = buildingConfig.Allotment.Center;
                //         pillarPosition.y += centerY;
                //         pillarPosition.x += pillarOrigin.x + pillarX * PillarWidthStep;
                //         pillarPosition.z += pillarOrigin.y + pillarY * PillarHeightStep;

                //         var mesh = MeshUtils.CubeMesh;
                //         var mtx = Matrix4x4.TRS(pillarPosition, Quaternion.identity, new Vector3(PillarThickness, floorConfig.Height - bias, PillarThickness));

                //         buildingInterior.Meshes.Add(mesh);
                //         buildingInterior.Matrices.Add(mtx);
                //     }
                // }
            }

            buildingInteriors.Add(buildingInterior);
        }

        return buildingInteriors;
    }
}