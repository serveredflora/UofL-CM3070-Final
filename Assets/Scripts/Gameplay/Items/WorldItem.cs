using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [Header("Settings")]
    public ItemDefinition ItemDefinition;

    public Item Item { get; private set; }

    private IPlayerInteractable PlayerInteractable;
    private WorldItemRepresentation InstantiatedPrefab;

    private void Start()
    {
        Item = new Item(ItemDefinition);

        PlayerInteractable = new WorldItemPlayerInteractable(Item.Definition.DisplayName, this);

        InstantiatedPrefab = Instantiate(Item.Definition.RepresentationPrefab, transform);
        InstantiatedPrefab.PlayerInteractableAccessor.Interactable = PlayerInteractable;
    }
}