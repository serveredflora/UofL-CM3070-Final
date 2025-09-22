using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildingInteriorPropsGenerator : ScriptableObject, IBuildingInteriorPropsGenerator
{
    public abstract List<BuildingInteriorProps> GenerateBuildingInteriorProps(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors);
}
