using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildingInteriorsGenerator : ScriptableObject, IBuildingInteriorsGenerator
{
    public abstract List<BuildingInterior> GenerateBuildingInteriors(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs);
}
