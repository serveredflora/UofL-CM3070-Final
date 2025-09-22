using UnityEngine;

public abstract class ARoadNetworkGenerator : ScriptableObject, IRoadNetworkGenerator
{
    public abstract RoadNetwork GenerateRoadNetwork(ProcGenGeneratorUtility generatorUtility);
}
