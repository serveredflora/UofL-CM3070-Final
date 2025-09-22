public struct Player
{
    public readonly PlayerCharacter Character;
    public readonly PlayerCharacterCollider CharacterCollider;
    public readonly PlayerCamera Camera;
    public readonly PlayerInteraction Interaction;
    public readonly PlayerInventory Inventory;
    public readonly PlayerEquipment Equipment;
    public readonly PlayerHotbar Hotbar;
    public readonly PlayerStats Stats;

    public Player(
                  PlayerCharacter character,
                  PlayerCharacterCollider characterCollider,
                  PlayerCamera camera,
                  PlayerInteraction interaction,
                  PlayerInventory inventory,
                  PlayerEquipment equipment,
                  PlayerHotbar hotbar,
                  PlayerStats stats)
    {
        Character = character;
        CharacterCollider = characterCollider;
        Camera = camera;
        Interaction = interaction;
        Inventory = inventory;
        Equipment = equipment;
        Hotbar = hotbar;
        Stats = stats;
    }
}
