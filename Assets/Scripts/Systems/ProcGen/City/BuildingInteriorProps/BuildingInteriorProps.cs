using System;
using System.Collections.Generic;

public class BuildingInteriorProps : IDisposable
{
    public Dictionary<BuildingRoom, List<BuildingInteriorProp>> Props = new();

    public void Dispose()
    {
        foreach (var propList in Props.Values)
        {
            propList.Dispose();
        }

        Props.Clear();
    }
}
