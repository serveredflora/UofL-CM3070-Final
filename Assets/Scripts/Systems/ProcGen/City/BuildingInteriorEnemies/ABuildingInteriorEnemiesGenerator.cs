using System.Collections.Generic;
using UnityEngine;

public abstract class ABuildingInteriorEnemiesGenerator : ScriptableObject, IBuildingInteriorEnemiesGenerator
{
    public abstract List<BuildingInteriorEnemies> GenerateBuildingInteriorEnemies(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors);
}
