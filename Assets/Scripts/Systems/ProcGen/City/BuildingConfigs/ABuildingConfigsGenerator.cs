using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildingConfigsGenerator : ScriptableObject, IBuildingConfigsGenerator
{
    public abstract List<BuildingConfig> GenerateBuildingConfigs(ProcGenGeneratorUtility generatorUtility, List<Allotment> allotments);
}