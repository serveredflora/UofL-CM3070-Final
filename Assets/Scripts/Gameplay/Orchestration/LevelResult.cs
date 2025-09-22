using System;
using UnityEngine;

public class LevelResult : IDisposable
{
    public Bounds CityBounds;
    public Bounds SpawnBounds;

    public GameObject ExitZoneGameObject;
    public StageExitZone ExitZone;

    public void Dispose()
    {
        UnityEngine.Object.Destroy(ExitZoneGameObject);
    }
}