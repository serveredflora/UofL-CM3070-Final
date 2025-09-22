using System.Collections.Generic;
using System.Drawing;

public interface IAllotmentsGenerator : IProcGenGenerator
{
    List<Allotment> GenerateAllotments(ProcGenGeneratorUtility generatorUtility, RoadNetwork roadNetwork);
}
