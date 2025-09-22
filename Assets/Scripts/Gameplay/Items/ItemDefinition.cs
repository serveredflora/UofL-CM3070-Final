using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Item", fileName = "Item")]
public class ItemDefinition : ScriptableObject
{
    [Header("Display")]
    public string DisplayName;
    public string DisplayDescription;

    [Header("Settings")]
    public Money SellValue;

    [Header("World")]
    public WorldItemRepresentation RepresentationPrefab;
}
