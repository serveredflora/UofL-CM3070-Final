using System;
using System.Collections.Generic;

public class BuildingInterior : IDisposable
{
    public Dictionary<BuildingFloorConfig, List<BuildingRoom>> RoomsByFloor = new();

    public void Dispose()
    {
        foreach (var rooms in RoomsByFloor.Values)
        {
            rooms.Dispose();
        }

        RoomsByFloor.Clear();
    }
}
