using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: make hotbar slots simply act as "bookmarks"/references to items in the inventory

public class PlayerHotbar : MonoBehaviour
{
    public event Action<int> OnSlotSelect;
    public event Action<int> OnSlotDeselect;

    public event Action OnPerformPrimaryAction;
    public event Action OnPerformSecondaryAction;

    private PlayerInventory Inventory;

    // TODO: support setting this at runtime
    public const int ActiveSlots = 2;

    public IReadOnlyList<Item> Items => _Items;
    private List<Item> _Items = new(ActiveSlots);

    private int CurrentlyActiveSlotIndex = -1;

    public void Initialize(PlayerInventory inventory)
    {
        Inventory = inventory;
    }

    public void UpdateInput(PlayerHotbarInput input)
    {
        UpdateSlotInput(input);
        UpdateActionInput(input);
    }

    private void UpdateSlotInput(PlayerHotbarInput input)
    {
        if (!input.AnySelectSlot)
        {
            return;
        }

        int requestedSlotIndex = -1;
        if (input.SelectSlotOne) requestedSlotIndex = 0;
        if (input.SelectSlotTwo) requestedSlotIndex = 1;
        if (input.SelectSlotThree) requestedSlotIndex = 2;
        if (input.SelectSlotFour) requestedSlotIndex = 3;

        // Ignore requests for slot indexes that are not active
        if (requestedSlotIndex >= ActiveSlots)
        {
            requestedSlotIndex = -1;
        }

        if (requestedSlotIndex != CurrentlyActiveSlotIndex)
        {
            if (CurrentlyActiveSlotIndex != -1)
            {
                OnSlotDeselect?.Invoke(CurrentlyActiveSlotIndex);
            }

            CurrentlyActiveSlotIndex = requestedSlotIndex;

            if (CurrentlyActiveSlotIndex != -1)
            {
                OnSlotSelect?.Invoke(CurrentlyActiveSlotIndex);
            }
        }
    }

    private void UpdateActionInput(PlayerHotbarInput input)
    {
        if (input.PerformPrimaryAction)
        {
            OnPerformPrimaryAction?.Invoke();
        }

        if (input.PerformSecondaryAction)
        {
            OnPerformSecondaryAction?.Invoke();
        }
    }
}
