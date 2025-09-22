using System;
using UnityEngine;

public class BuildingCeiling : IDisposable
{
    public GameObject GameObjectInstance;

    public void Dispose()
    {
        GameObject.Destroy(GameObjectInstance);
    }
}
