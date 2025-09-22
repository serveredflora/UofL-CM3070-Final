using UnityEngine;

public class WorldButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeReference]
    private IWorldButtonPlayerInteractableAction[] InteractableActions;

    [Header("References")]
    [SerializeField]
    private PlayerInteractableAccessor PlayerInteractableAccessor;

    private IPlayerInteractable PlayerInteractable;

    private void Start()
    {
        PlayerInteractable = new SimplePlayerInteractable("Button", InteractableActions);
        PlayerInteractableAccessor.Interactable = PlayerInteractable;
    }
}