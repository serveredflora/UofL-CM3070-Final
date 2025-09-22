public interface IPlayerInteractableAction
{
    string Info { get; }
    PlayerInteractableActionPerformResult Perform(Player player);
}
