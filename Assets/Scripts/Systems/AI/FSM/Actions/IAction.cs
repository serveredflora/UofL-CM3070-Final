namespace FSM
{
    public interface IAction
    {
        void Perform(IFiniteStateMachineStorage storage);
    }
}