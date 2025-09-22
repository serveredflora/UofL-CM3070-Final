using System;

public class PlayerInventoryMoney : IDisposable
{
    public event Action OnChange;

    public Money TotalMoney { get; private set; } = Money.Zero;

    private readonly PlayerInventory Inventory;

    public PlayerInventoryMoney(PlayerInventory inventory)
    {
        Inventory = inventory;

        Inventory.OnItemAdd += InventoryItemAdded;
        Inventory.OnItemRemove += InventoryItemRemoved;

        foreach (var item in Inventory.Items)
        {
            TotalMoney += item.Definition.SellValue;
        }
    }

    public void Dispose()
    {
        Inventory.OnItemAdd -= InventoryItemAdded;
        Inventory.OnItemRemove -= InventoryItemRemoved;
    }

    private void InventoryItemAdded(Item item)
    {
        TotalMoney += item.Definition.SellValue;
        OnChange?.Invoke();
    }

    private void InventoryItemRemoved(Item item)
    {
        TotalMoney -= item.Definition.SellValue;
        OnChange?.Invoke();
    }
}