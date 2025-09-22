namespace FSM
{
    public interface IFiniteStateMachineStorage
    {
        T Read<T>();
        bool TryRead<T>(out T value);

        void Write<T>(T value);
    }
}