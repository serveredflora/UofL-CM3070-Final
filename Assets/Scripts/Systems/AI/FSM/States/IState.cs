using System.Collections.Generic;

namespace FSM
{
    public interface IState
    {
        IReadOnlyList<IAction> Actions { get; }
        IAction EntryAction { get; }
        IAction ExitAction { get; }
        IReadOnlyList<ITransition> Transitions { get; }
    }
}