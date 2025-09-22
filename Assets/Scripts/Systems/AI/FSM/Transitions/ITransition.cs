namespace FSM
{
    public interface ITransition
    {
        ICondition Decision { get; }
        IState TargetState { get; }
        IAction Action { get; }

        bool IsTriggered(IFiniteStateMachineStorage storage) => Decision.Evaluate(storage);
    }
}