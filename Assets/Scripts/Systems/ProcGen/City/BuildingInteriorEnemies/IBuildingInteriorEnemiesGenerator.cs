using System.Collections.Generic;

public interface IBuildingInteriorEnemiesGenerator : IProcGenGenerator
{
    List<BuildingInteriorEnemies> GenerateBuildingInteriorEnemies(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors);
}
