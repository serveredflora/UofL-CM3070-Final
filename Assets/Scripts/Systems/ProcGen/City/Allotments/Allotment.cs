using System;
using UnityEngine;

public class Allotment : IDisposable
{
    public AllotmentType Type;
    public Bounds Bounds;
    public GameObject GameObjectInstance;

    public void Dispose()
    {
        GameObject.Destroy(GameObjectInstance);
    }
}
