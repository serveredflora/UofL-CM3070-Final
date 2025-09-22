using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRoom : IDisposable
{
    public Bounds Bounds;
    public List<BuildingWall> Walls = new();
    public List<BuildingRoomDetail> Details = new();

    public void Dispose()
    {
        Walls.Dispose();
        Details.Dispose();
    }
}
