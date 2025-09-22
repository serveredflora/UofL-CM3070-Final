using System.Collections.Generic;
using TriInspector;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

[CreateAssetMenu(menuName = "ProcGen/BuildingInteriorProps/Simple", fileName = "SimpleBuildingInteriorPropsGenerator")]
public class SimpleBuildingInteriorPropsGenerator : ABuildingInteriorPropsGenerator
{
    [Header("Settings")]
    public float MinPaddingFromWall = 1.0f;
    public float MinDistanceBetweenProps = 1.0f;

    [Header("References")]
    [Required]
    [AssetsOnly]
    public WorldItem WorldItemPrefab;
    public List<ItemDefinition> ItemDefinitions = new();

    private const float SpawnSafetyMargin = 0.1f;

    public override List<BuildingInteriorProps> GenerateBuildingInteriorProps(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors)
    {
        var categoryGameObj = generatorUtility.RequestCategoryGameObject("InteriorProps");

        List<BuildingInteriorProps> buildingInteriorPropsList = new();

        for (int configIdx = 0; configIdx < buildingConfigs.Count; ++configIdx)
        {
            var buildingConfig = buildingConfigs[configIdx];
            var buildingInterior = buildingInteriors[configIdx];

            for (int floorIdx = 0; floorIdx < buildingConfig.FloorConfigs.Count; ++floorIdx)
            {
                var floorConfig = buildingConfig.FloorConfigs[floorIdx];
                var floorRooms = buildingInterior.RoomsByFloor[floorConfig];

                var buildingInteriorProps = new BuildingInteriorProps();

                for (int roomIdx = 0; roomIdx < floorRooms.Count; ++roomIdx)
                {
                    var room = floorRooms[roomIdx];

                    var selectedItemDef = ItemDefinitions.PickRandom();

                    var spawnPos = room.Bounds.center.WithY(room.Bounds.GetBottom() + floorConfig.FlooringThickness - selectedItemDef.RepresentationPrefab.Bounds.GetBottom() + SpawnSafetyMargin);

                    var prop = new BuildingInteriorProp();
                    if (!buildingInteriorProps.Props.TryGetValue(room, out var propsList))
                    {
                        propsList = new();
                        buildingInteriorProps.Props[room] = propsList;
                    }

                    propsList.Add(prop);

                    var itemGameObj = GameObject.Instantiate(WorldItemPrefab, spawnPos, Quaternion.identity, categoryGameObj.transform);
                    prop.GameObjectInstance = itemGameObj.gameObject;

                    itemGameObj.ItemDefinition = selectedItemDef;
                }

                buildingInteriorPropsList.Add(buildingInteriorProps);
            }
        }

        return buildingInteriorPropsList;
    }
}