using System.Collections.Generic;

public interface IPlayerItem
{
    string Name { get; }
    IEnumerable<IPlayerItemAction> Actions { get; }
}
