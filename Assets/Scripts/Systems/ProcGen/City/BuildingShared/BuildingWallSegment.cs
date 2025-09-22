using System;
using UnityEngine;

public class BuildingWallSegment : IDisposable
{
    public GameObject GameObjectInstance;

    public void Dispose()
    {
        GameObject.Destroy(GameObjectInstance);
    }
}
