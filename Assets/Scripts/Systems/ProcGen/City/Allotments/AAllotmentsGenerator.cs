using System.Collections.Generic;
using UnityEngine;

public abstract class AAllotmentsGenerator : ScriptableObject, IAllotmentsGenerator
{
    public abstract List<Allotment> GenerateAllotments(ProcGenGeneratorUtility generatorUtility, RoadNetwork roadNetwork);
}
