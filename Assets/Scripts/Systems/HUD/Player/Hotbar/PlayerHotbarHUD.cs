using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHotbarHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerHotbar PlayerHotbar;
    [SerializeField]
    private RectTransform PanelTransform;
    [SerializeField]
    private HorizontalLayoutGroup LayoutGroup;
    [SerializeField]
    private PlayerHotbarHUDSlot SlotPrefab;

    private PlayerHotbarHUDSlot[] Slots;

    private void Start()
    {
        Slots = new PlayerHotbarHUDSlot[PlayerHotbar.ActiveSlots];
        for (int i = 0; i < Slots.Length; ++i)
        {
            Slots[i] = CreateSlot(null);
        }

        PlayerHotbar.OnSlotSelect += HotbarSlotSelected;
        PlayerHotbar.OnSlotDeselect += HotbarSlotDeselected;
    }

    private void OnDestroy()
    {
        PlayerHotbar.OnSlotSelect -= HotbarSlotSelected;
        PlayerHotbar.OnSlotDeselect -= HotbarSlotDeselected;
    }

    private void HotbarSlotSelected(int index)
    {
        // Slots[i] = CreateSlot(item);
    }

    private void HotbarSlotDeselected(int index)
    {
        // ItemToGridCell.Remove(item);
    }

    private PlayerHotbarHUDSlot CreateSlot(Item item)
    {
        var cell = Instantiate(SlotPrefab, LayoutGroup.transform);
        cell.Item = item;

        return cell;
    }
}
