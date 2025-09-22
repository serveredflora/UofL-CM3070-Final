using System;
using System.Collections.Generic;

public class BuildingWall : IDisposable
{
    public List<BuildingWallSegment> WallSegments = new();
    public List<BuildingDoor> Doors = new();

    public void Dispose()
    {
        WallSegments.Dispose();
        Doors.Dispose();
    }
}
