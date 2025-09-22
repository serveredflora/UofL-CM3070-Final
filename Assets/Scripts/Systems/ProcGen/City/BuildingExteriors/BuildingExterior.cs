using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingExterior : IDisposable
{
    public List<BuildingFloor> Floors = new();

    public void Dispose()
    {
        Floors.Dispose();
    }
}
