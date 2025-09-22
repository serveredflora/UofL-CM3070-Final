using System;
using UnityEngine;

public class BuildingFlooring : IDisposable
{
    public GameObject GameObjectInstance;

    public void Dispose()
    {
        GameObject.Destroy(GameObjectInstance);
    }
}
