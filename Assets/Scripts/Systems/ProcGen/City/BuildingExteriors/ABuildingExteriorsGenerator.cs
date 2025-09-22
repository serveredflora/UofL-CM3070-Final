using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildingExteriorsGenerator : ScriptableObject, IBuildingExteriorsGenerator
{
    public abstract List<BuildingExterior> GenerateBuildingExteriors(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors);
}