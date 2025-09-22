using System.Collections.Generic;
using TriInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ProcGen/BuildingInteriorEnemies/Simple", fileName = "SimpleBuildingInteriorEnemiesGenerator")]
public class SimpleBuildingInteriorEnemiesGenerator : ABuildingInteriorEnemiesGenerator
{
    [Header("Settings")]
    public float MinPaddingFromWall = 1.0f;
    public float MinDistanceBetweenEnemies = 1.0f;
    public Vector2Int WaypointCountRange = new(2, 5);

    [Header("References")]
    [Required]
    [AssetsOnly]
    public AIAgent EnemyAIAgentPrefab;

    public override List<BuildingInteriorEnemies> GenerateBuildingInteriorEnemies(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors)
    {
        var categoryGameObj = generatorUtility.RequestCategoryGameObject("InteriorEnemies");

        List<BuildingInteriorEnemies> buildingInteriorEnemiesList = new();

        var playerManager = GameObject.FindAnyObjectByType<PlayerManager>();
        IAIAgentTarget playerTarget = playerManager != null ? new AIAgentPlayerCharacterTarget(playerManager.Player) : new AIAgentEmptyTarget();

        for (int configIdx = 0; configIdx < buildingConfigs.Count; ++configIdx)
        {
            var buildingConfig = buildingConfigs[configIdx];
            var buildingInterior = buildingInteriors[configIdx];

            for (int floorIdx = 0; floorIdx < buildingConfig.FloorConfigs.Count; ++floorIdx)
            {
                var floorConfig = buildingConfig.FloorConfigs[floorIdx];
                var floorRooms = buildingInterior.RoomsByFloor[floorConfig];

                var buildingInteriorEnemies = new BuildingInteriorEnemies();

                for (int roomIdx = 0; roomIdx < floorRooms.Count; ++roomIdx)
                {
                    var room = floorRooms[roomIdx];

                    var spawnPos = GetRoomFloorCenter(room, floorConfig);

                    var enemy = new BuildingInteriorEnemy();
                    if (!buildingInteriorEnemies.EnemiesByRoom.TryGetValue(room, out var enemiesList))
                    {
                        enemiesList = new();
                        buildingInteriorEnemies.EnemiesByRoom[room] = enemiesList;
                    }

                    enemiesList.Add(enemy);

                    var agent = GameObject.Instantiate(EnemyAIAgentPrefab, spawnPos, Quaternion.identity, categoryGameObj.transform);
                    enemy.AgentInstance = agent;

                    agent.Target = playerTarget;

                    // TODO: set better patrol waypoints
                    var waypoints = new List<Vector3>() { spawnPos };
                    agent.Waypoints = waypoints;

                    int waypointCount = Random.Range(WaypointCountRange.x, WaypointCountRange.y);
                    waypointCount = Mathf.Min(waypointCount, floorRooms.Count);
                    List<int> selectedRooms = new() { roomIdx };
                    for (int waypointIndex = 1; waypointIndex < waypointCount; ++waypointIndex)
                    {
                        int idx;
                        while (true)
                        {
                            idx = Random.Range(0, floorRooms.Count);
                            if (!selectedRooms.Contains(idx))
                            {
                                break;
                            }
                        }

                        Vector3 waypointPos = GetRoomFloorCenter(floorRooms[idx], floorConfig);
                        waypoints.Add(waypointPos);
                        selectedRooms.Add(idx);
                    }
                }

                buildingInteriorEnemiesList.Add(buildingInteriorEnemies);
            }
        }

        return buildingInteriorEnemiesList;
    }

    private Vector3 GetRoomFloorCenter(BuildingRoom room, BuildingFloorConfig floorConfig)
    {
        return room.Bounds.center.WithY(room.Bounds.GetBottom() + floorConfig.FlooringThickness);
    }
}
