using System.Collections.Generic;
using TriInspector;
using Unity.AI.Navigation;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("Settings")]
    public int RandomSeed = 0;

    [Space(10)]
    [Required]
    [AssetsOnly]
    public ARoadNetworkGenerator RoadNetworkGenerator;
    [Required]
    [AssetsOnly]
    public AAllotmentsGenerator AllotmentsGenerator;
    [Required]
    [AssetsOnly]
    public ABuildingConfigsGenerator BuildingConfigsGenerator;
    [Required]
    [AssetsOnly]
    public ABuildingExteriorsGenerator BuildingExteriorsGenerator;
    [Required]
    [AssetsOnly]
    public ABuildingInteriorsGenerator BuildingInteriorsGenerator;
    [Required]
    [AssetsOnly]
    public ABuildingInteriorPropsGenerator BuildingInteriorPropsGenerator;
    [Required]
    [AssetsOnly]
    public ABuildingInteriorEnemiesGenerator BuildingInteriorEnemiesGenerator;

    [Header("References")]
    public NavMeshSurface NavMeshSurface;

    private bool IsGenerated = false;

    private ProcGenGeneratorUtility GeneratorUtility;

    private RoadNetwork RoadNetwork;
    private List<Allotment> Allotments;
    private List<BuildingConfig> BuildingConfigs;
    private List<BuildingExterior> BuildingExteriors;
    private List<BuildingInterior> BuildingInteriors;
    private List<BuildingInteriorProps> BuildingInteriorProps;
    private List<BuildingInteriorEnemies> BuildingInteriorEnemies;

    private void Awake()
    {
        GeneratorUtility = new ProcGenGeneratorUtility(gameObject);
    }

    private void OnDestroy()
    {
        GeneratorUtility.Dispose();
    }

    private void Update()
    {
        if (!IsGenerated)
        {
            return;
        }

        // DebugDraw();
    }

    private void DebugDraw()
    {
        void DrawBounds(Bounds bounds, Color color)
        {
            // ALINE usage commented out
            // Drawing.Draw.SolidBox(bounds, Color.Lerp(color, Color.black, 0.0f).WithA(0.25f));
            // Drawing.Draw.WireBox(bounds, color);
        }

        void DrawCircle(Vector3 pos, Vector3 norm, float radius, Color color)
        {
            // ALINE usage commented out
            // Drawing.Draw.SolidCircle(pos, norm, radius, color.WithA(0.25f));
            // Drawing.Draw.Circle(pos, norm, radius, color);
        }

        void DrawLabelLineSegment(Vector3 a, Vector3 b, string text, Color color)
        {
            DrawCircle(a, Vector3.up, 0.25f, color);
            DrawCircle(b, Vector3.up, 0.25f, color);

            // ALINE usage commented out
            // Drawing.Draw.Line(a, b, color);
            // Drawing.Draw.SolidBox(Vector3.Lerp(a, b, 0.5f), new Vector3(0.9f, 0.4f, 0.1f), Color.black);
            // Drawing.Draw.Label3D(Vector3.Lerp(a, b, 0.5f), Quaternion.identity, text, 0.15f, LabelAlignment.Center, color);
        }

        void DrawFloorConfig(BuildingConfig buildingConfig, Color color)
        {
            Vector3 origin = buildingConfig.Bounds.center.WithY(buildingConfig.Bounds.GetBottom());

            Vector3 floorStart = origin;

            for (int i = 0; i < buildingConfig.FloorConfigs.Count; ++i)
            {
                var floorConfig = buildingConfig.FloorConfigs[i];

                Vector3 segmentStart = floorStart;
                Vector3 segmentEnd = floorStart;

                segmentEnd += Vector3.up * floorConfig.FlooringThickness;
                DrawLabelLineSegment(segmentStart, segmentEnd, "Flooring", color);
                segmentStart = segmentEnd;

                segmentEnd += Vector3.up * floorConfig.InteriorHeight;
                DrawLabelLineSegment(segmentStart, segmentEnd, "Interior", color);
                segmentStart = segmentEnd;

                segmentEnd += Vector3.up * floorConfig.CeilingThickness;
                DrawLabelLineSegment(segmentStart, segmentEnd, "Ceiling", color);

                floorStart += Vector3.up * floorConfig.ExteriorHeight;
            }
        }

        using (Drawing.Draw.WithLineWidth(1.0f))
        {
            foreach (var allotment in Allotments)
            {
                DrawBounds(allotment.Bounds, Color.Lerp(Color.green, Color.yellow, 0.5f));
            }

            foreach (var buildingConfig in BuildingConfigs)
            {
                DrawBounds(buildingConfig.Bounds, Color.Lerp(Color.red, Color.yellow, 0.5f));
                DrawFloorConfig(buildingConfig, Color.Lerp(Color.magenta, Color.blue, 0.5f));
            }

            foreach (var buildingExterior in BuildingExteriors)
            {
                foreach (var floor in buildingExterior.Floors)
                {
                    DrawBounds(floor.Bounds, Color.Lerp(Color.magenta, Color.red, 0.5f));
                }
            }

            foreach (var buildingInterior in BuildingInteriors)
            {
                foreach (var floorRooms in buildingInterior.RoomsByFloor.Values)
                {
                    foreach (var room in floorRooms)
                    {
                        DrawBounds(room.Bounds, Color.Lerp(Color.magenta, Color.cyan, 0.5f));
                    }
                }
            }
        }
    }

    [Button]
    [EnableInPlayMode]
    [DisableIf(nameof(IsGenerated))]
    public void Generate()
    {
        if (IsGenerated)
        {
            return;
        }

        Random.InitState(RandomSeed);

        RoadNetwork = RoadNetworkGenerator.GenerateRoadNetwork(GeneratorUtility);
        Allotments = AllotmentsGenerator.GenerateAllotments(GeneratorUtility, RoadNetwork);
        BuildingConfigs = BuildingConfigsGenerator.GenerateBuildingConfigs(GeneratorUtility, Allotments);
        BuildingInteriors = BuildingInteriorsGenerator.GenerateBuildingInteriors(GeneratorUtility, BuildingConfigs);
        BuildingExteriors = BuildingExteriorsGenerator.GenerateBuildingExteriors(GeneratorUtility, BuildingConfigs, BuildingInteriors);
        BuildingInteriorProps = BuildingInteriorPropsGenerator.GenerateBuildingInteriorProps(GeneratorUtility, BuildingConfigs, BuildingInteriors);

        NavMeshSurface.BuildNavMesh();

        BuildingInteriorEnemies = BuildingInteriorEnemiesGenerator.GenerateBuildingInteriorEnemies(GeneratorUtility, BuildingConfigs, BuildingInteriors);

        IsGenerated = true;
    }

    [Button]
    [EnableInPlayMode]
    [EnableIf(nameof(IsGenerated))]
    public void Cleanup()
    {
        if (!IsGenerated)
        {
            return;
        }

        BuildingInteriorEnemies.Dispose();

        NavMeshSurface.RemoveData();

        BuildingInteriorProps.Dispose();
        BuildingInteriors.Dispose();
        BuildingExteriors.Dispose();
        Allotments.Dispose();
        RoadNetwork.Dispose();

        IsGenerated = false;
    }

    public bool TryGetBounds(out Bounds bounds)
    {
        if (!IsGenerated)
        {
            bounds = default(Bounds);
            return false;
        }

        bounds = new();
        foreach (var p in RoadNetwork.Nodes)
        {
            bounds.Encapsulate(p.Point);
        }
        bounds.ExpandXZ(RoadNetwork.Thickness);

        return true;
    }
}
