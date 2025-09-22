using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFloor : IDisposable
{
    public Bounds Bounds;

    public BuildingFlooring Flooring;
    public BuildingCeiling Ceiling;
    public List<BuildingWall> Walls = new();

    public void Dispose()
    {
        Flooring.Dispose();
        Ceiling.Dispose();
        Walls.Dispose();
    }
}