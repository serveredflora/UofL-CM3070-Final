using System;
using PrimeTween;
using UnityEngine;

[Serializable]
public class WorldDoorToggleOpenPlayerInteractableAction : IWorldDoorPlayerInteractableAction
{
    [Header("References")]
    [SerializeField]
    private WorldDoor Door;

    public string Info => Door.IsOpen ? "Close" : "Open";

    public PlayerInteractableActionPerformResult Perform(Player player)
    {
        Door.ToggleOpen();
        return new PlayerInteractableActionPerformResult(true);
    }
}

public class WorldDoor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeReference]
    private IWorldDoorPlayerInteractableAction[] InteractableActions;

    [Header("References")]
    [SerializeField]
    private Transform HingeTransform;
    [SerializeField]
    private PlayerInteractableAccessor PlayerInteractableAccessor;

    private IPlayerInteractable PlayerInteractable;

    public bool IsOpen { get; private set; }

    private void Start()
    {
        PlayerInteractable = new SimplePlayerInteractable("Door", InteractableActions);
        PlayerInteractableAccessor.Interactable = PlayerInteractable;
    }

    public void ToggleOpen()
    {
        if (!IsOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        if (IsOpen)
        {
            return;
        }

        Tween.LocalRotation(HingeTransform, endValue: Quaternion.Euler(0.0f, -90.0f, 0.0f), duration: 1.5f, ease: Ease.OutQuint);

        IsOpen = true;
    }

    public void Close()
    {
        if (!IsOpen)
        {
            return;
        }

        Tween.LocalRotation(HingeTransform, endValue: Quaternion.identity, duration: 1.5f, ease: Ease.OutQuint);

        IsOpen = false;
    }
}