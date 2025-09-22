using System;
using UnityEngine;

public class BuildingDoor : IDisposable
{
    public Bounds Bounds;
    public GameObject GameObjectInstance;

    public void Dispose()
    {
        GameObject.Destroy(GameObjectInstance);
    }
}