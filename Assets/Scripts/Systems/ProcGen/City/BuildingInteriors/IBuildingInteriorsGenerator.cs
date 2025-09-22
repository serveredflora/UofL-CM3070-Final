using System.Collections.Generic;

public interface IBuildingInteriorsGenerator : IProcGenGenerator
{
    List<BuildingInterior> GenerateBuildingInteriors(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs);
}