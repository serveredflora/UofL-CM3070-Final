using System;
using UnityEngine;

public class BuildingRoomDetail : IDisposable
{
    public GameObject GameObjectInstance;

    public void Dispose()
    {
        GameObject.Destroy(GameObjectInstance);
    }
}