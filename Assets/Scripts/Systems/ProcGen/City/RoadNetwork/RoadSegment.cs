using System;
using UnityEngine;

public class RoadSegment : IDisposable
{
    public GameObject GameObjectInstance;

    public void Dispose()
    {
        GameObject.Destroy(GameObjectInstance);
    }
}
