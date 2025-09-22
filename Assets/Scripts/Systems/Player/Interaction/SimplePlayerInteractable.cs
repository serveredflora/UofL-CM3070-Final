using System.Collections.Generic;
using System.Linq;

public class SimplePlayerInteractable : IPlayerInteractable
{
    public string Name { get; }

    public IReadOnlyList<IPlayerInteractableAction> Actions => _Actions;
    private readonly IPlayerInteractableAction[] _Actions;

    public SimplePlayerInteractable(string name, IEnumerable<IPlayerInteractableAction> actions)
    {
        Name = name;
        _Actions = actions.ToArray();
    }

    public SimplePlayerInteractable(string name, params IPlayerInteractableAction[] actions)
    {
        Name = name;
        _Actions = actions;
    }
}
