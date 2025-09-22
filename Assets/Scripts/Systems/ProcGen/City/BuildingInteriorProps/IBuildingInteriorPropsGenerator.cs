using System.Collections.Generic;

public interface IBuildingInteriorPropsGenerator : IProcGenGenerator
{
    List<BuildingInteriorProps> GenerateBuildingInteriorProps(ProcGenGeneratorUtility generatorUtility, List<BuildingConfig> buildingConfigs, List<BuildingInterior> buildingInteriors);
}
