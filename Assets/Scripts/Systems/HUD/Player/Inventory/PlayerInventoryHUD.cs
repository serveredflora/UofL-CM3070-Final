using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerInventory PlayerInventory;
    [SerializeField]
    private RectTransform PanelTransform;
    [SerializeField]
    private Button CloseButton;
    [SerializeField]
    private ScrollRect ScrollRect;
    [SerializeField]
    private GridLayoutGroup GridLayoutGroup;
    [SerializeField]
    private PlayerInteractionHUDGridCell GridCellPrefab;

    private readonly Dictionary<Item, PlayerInteractionHUDGridCell> ItemToGridCell = new();

    private bool IsMenuOpen = false;

    private void Start()
    {
        GridLayoutGroup.constraintCount = PlayerInventory.GridCellsX;

        PanelTransform.gameObject.SetActive(false);

        CloseButton.onClick.AddListener(CloseButtonClick);

        PlayerInventory.OnItemAdd += InventoryItemAdded;
        PlayerInventory.OnItemRemove += InventoryItemRemoved;

        PlayerInventory.OnToggleMenu += InventoryToggleMenu;
    }

    private void OnDestroy()
    {
        CloseButton.onClick.RemoveListener(CloseButtonClick);

        PlayerInventory.OnItemAdd -= InventoryItemAdded;
        PlayerInventory.OnItemRemove -= InventoryItemRemoved;

        PlayerInventory.OnToggleMenu -= InventoryToggleMenu;
    }

    public void ToggleMenu()
    {
        if (!IsMenuOpen)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        if (IsMenuOpen)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.None;

        ScrollRect.verticalNormalizedPosition = 0.0f;
        PanelTransform.gameObject.SetActive(true);

        IsMenuOpen = true;
    }

    public void CloseMenu()
    {
        if (!IsMenuOpen)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;

        ScrollRect.StopMovement();
        PanelTransform.gameObject.SetActive(false);

        IsMenuOpen = false;
    }

    private void CloseButtonClick()
    {
        CloseMenu();
    }

    private void InventoryItemAdded(Item item)
    {
        ItemToGridCell[item] = CreateGridCell(item);
    }

    private void InventoryItemRemoved(Item item)
    {
        ItemToGridCell.Remove(item);
    }

    private void InventoryToggleMenu()
    {
        ToggleMenu();
    }

    private PlayerInteractionHUDGridCell CreateGridCell(Item item)
    {
        var cell = Instantiate(GridCellPrefab, GridLayoutGroup.transform);
        cell.Item = item;

        return cell;
    }
}
