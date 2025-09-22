using System.Collections.Generic;

public interface IPlayerInteractable
{
    string Name { get; }
    IReadOnlyList<IPlayerInteractableAction> Actions { get; }
}
