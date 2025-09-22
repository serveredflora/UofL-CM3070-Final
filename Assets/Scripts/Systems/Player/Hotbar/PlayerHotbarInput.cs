public struct PlayerHotbarInput
{
    public bool SelectSlotOne;
    public bool SelectSlotTwo;
    public bool SelectSlotThree;
    public bool SelectSlotFour;

    public bool PerformPrimaryAction;
    public bool PerformSecondaryAction;

    public bool AnySelectSlot => SelectSlotOne || SelectSlotTwo || SelectSlotThree || SelectSlotFour;
}