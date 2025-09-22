using System;
using TriInspector;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [Required]
    [SceneObjectsOnly]
    public CityGenerator CityGenerator;
    [Required]
    [AssetsOnly]
    public GameObject ExitZonePrefab;

    public LevelResult Generate(int? seed)
    {
        if (seed.HasValue)
        {
            CityGenerator.RandomSeed = seed.Value;
        }

        var result = new LevelResult();

        CityGenerator.Generate();
        if (!CityGenerator.TryGetBounds(out result.CityBounds))
        {
            throw new InvalidOperationException();
        }

        // TODO: spawn bounds
        result.SpawnBounds = new Bounds(new Vector3(0.0f, 0.0f, 0.0f) + Vector3.up, Vector3.one);

        var cityBoundsCorners = result.CityBounds.GetCorners();

        // Only interested in bottom corners, assemble them in a clockwise-ordered array
        var cityBoundsRectPoints = new Vector3[] {
            cityBoundsCorners.RightBottomBack,
            cityBoundsCorners.RightBottomFront,
            cityBoundsCorners.LeftBottomFront,
            cityBoundsCorners.LeftBottomBack,
        };

        var exitZoneEdgeIdx = UnityEngine.Random.Range(0, cityBoundsRectPoints.Length);
        var exitZoneEdgePointAIdx = exitZoneEdgeIdx;
        var exitZoneEdgePointBIdx = (exitZoneEdgeIdx + 1) % cityBoundsRectPoints.Length;

        var exitZoneEdgePointA = cityBoundsRectPoints[exitZoneEdgePointAIdx];
        var exitZoneEdgePointB = cityBoundsRectPoints[exitZoneEdgePointBIdx];

        var exitZoneEdgeDiff = exitZoneEdgePointB - exitZoneEdgePointA;

        var exitZonePoint = Vector3.Lerp(exitZoneEdgePointA, exitZoneEdgePointB, UnityEngine.Random.value);
        var exitZoneAngle = exitZoneEdgeDiff.ToVec2XZ().normalized.GetSignedAngle();
        exitZoneAngle -= 90.0f; // To get the perpendicular angle

        var exitZonePosition = exitZonePoint;
        var exitZoneRotation = Quaternion.Euler(0.0f, exitZoneAngle, 0.0f);

        var exitZoneGameObject = Instantiate(ExitZonePrefab, exitZonePosition, exitZoneRotation, transform);
        result.ExitZoneGameObject = exitZoneGameObject;
        result.ExitZone = exitZoneGameObject.GetComponentInChildren<StageExitZone>();

        return result;
    }

    public void Cleanup()
    {
        CityGenerator.Cleanup();
    }
}