using System.Collections.Generic;

public interface IBuildingExteriorsGenerator : IProcGenGenerator
{
    List<BuildingExterior> GenerateBuildingExteriors(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors);
}
