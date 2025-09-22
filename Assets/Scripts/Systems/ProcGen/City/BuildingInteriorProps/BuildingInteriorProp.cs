using System;
using UnityEngine;

public class BuildingInteriorProp : IDisposable
{
    public GameObject GameObjectInstance;

    public void Dispose()
    {
        if (GameObjectInstance != null)
        {
            GameObject.Destroy(GameObjectInstance);
        }
    }
}