public interface IPlayerItemAction
{
    string Info { get; }
    PlayerItemActionInputSlot InputSlot { get; }
    PlayerItemActionInputUsage InputUsage { get; }
}
