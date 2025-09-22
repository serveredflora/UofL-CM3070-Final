using UnityEngine;

public class WorldItemPlayerInteractable : SimplePlayerInteractable
{
    public WorldItemPlayerInteractable(string name, WorldItem worldItem) : base(name,
        new[] {
            new PickupAction(worldItem),
        })
    { }

    private class PickupAction : IPlayerInteractableAction
    {
        public string Info => _Info;
        private readonly string _Info;

        private WorldItem WorldItem;

        public PickupAction(WorldItem worldItem)
        {
            WorldItem = worldItem;
            _Info = $"Pickup {WorldItem.Item.Definition.SellValue}";
        }

        public PlayerInteractableActionPerformResult Perform(Player player)
        {
            bool canStoreItem = player.Inventory.HasEnoughSpace(WorldItem.Item);

            PlayerInteractableActionPerformResult result;
            if (canStoreItem)
            {
                player.Inventory.Add(WorldItem.Item);
                GameObject.Destroy(WorldItem.gameObject);
                result = new PlayerInteractableActionPerformResult(canStoreItem);
            }
            else
            {
                result = new PlayerInteractableActionPerformResult(canStoreItem, "Not enough space");
            }

            return result;
        }
    }
}
