using System;
using System.Collections.Generic;

public class BuildingInteriorEnemies : IDisposable
{
    public Dictionary<BuildingRoom, List<BuildingInteriorEnemy>> EnemiesByRoom = new();

    public void Dispose()
    {
        foreach (var enemies in EnemiesByRoom.Values)
        {
            enemies.Dispose();
        }

        EnemiesByRoom.Clear();
    }
}
