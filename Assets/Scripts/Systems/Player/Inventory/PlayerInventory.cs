using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public event Action<Item> OnItemAdd;
    public event Action<Item> OnItemRemove;

    public event Action OnToggleMenu;

    // TODO: support changing the grid cell size at runtime (eg. for upgrades, level ups or difficulty)
    public const int GridCellsX = 5;
    public const int GridCellsY = 3;
    public const int MaxItems = GridCellsX * GridCellsY;

    public IReadOnlyList<Item> Items => _Items;
    private List<Item> _Items = new(MaxItems);

    public void Initialize()
    {
        // ...
    }

    public void UpdateInput(PlayerInventoryInput input)
    {
        if (input.ToggleMenu)
        {
            OnToggleMenu?.Invoke();
        }
    }

    public void Add(Item item)
    {
        Debug.Assert(_Items.Count < MaxItems);
        Debug.Assert(!_Items.Contains(item));

        _Items.Add(item);
        OnItemAdd?.Invoke(item);
    }

    public void Remove(Item item)
    {
        Debug.Assert(_Items.Count > 0);
        Debug.Assert(_Items.Contains(item));

        _Items.Remove(item);
        OnItemRemove?.Invoke(item);
    }

    public bool HasEnoughSpace(Item item)
    {
        return _Items.Count < MaxItems;
    }
}
