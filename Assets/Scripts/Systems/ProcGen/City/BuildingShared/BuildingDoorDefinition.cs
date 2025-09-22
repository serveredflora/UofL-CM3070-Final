using UnityEngine;

[CreateAssetMenu(menuName = "ProcGen/BuildingShared/DoorDefinition", fileName = "DoorDefinition")]
public class BuildingDoorDefinition : ScriptableObject
{
    public GameObject Prefab;
    public float Width;
    public float Height;
}
