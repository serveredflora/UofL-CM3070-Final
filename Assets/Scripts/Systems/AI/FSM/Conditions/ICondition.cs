namespace FSM
{
    public interface ICondition
    {
        bool Evaluate(IFiniteStateMachineStorage storage);
    }
}