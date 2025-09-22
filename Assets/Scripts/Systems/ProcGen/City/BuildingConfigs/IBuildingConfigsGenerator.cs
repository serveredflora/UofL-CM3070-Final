using System.Collections.Generic;

public interface IBuildingConfigsGenerator : IProcGenGenerator
{
    List<BuildingConfig> GenerateBuildingConfigs(ProcGenGeneratorUtility generatorUtility, List<Allotment> allotments);
}
