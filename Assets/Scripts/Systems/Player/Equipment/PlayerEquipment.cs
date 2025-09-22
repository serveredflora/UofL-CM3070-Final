using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private PlayerInventory Inventory;

    public void Initialize(PlayerInventory inventory)
    {
        Inventory = inventory;
    }
}
