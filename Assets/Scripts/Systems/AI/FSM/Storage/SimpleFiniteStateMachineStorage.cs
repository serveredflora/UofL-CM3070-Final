using System;
using System.Collections.Generic;

namespace FSM
{
    public class SimpleFiniteStateMachineStorage : IFiniteStateMachineStorage
    {
        private readonly Dictionary<Type, object> Entries = new();

        public T Read<T>()
        {
            if (!TryRead(out T value))
            {
                throw new InvalidOperationException();
            }

            return value;
        }

        public bool TryRead<T>(out T value)
        {
            if (!Entries.TryGetValue(typeof(T), out object obj))
            {
                value = default;
                return false;
            }

            value = (T)obj;
            return true;
        }

        public void Write<T>(T value)
        {
            Entries[typeof(T)] = value;
        }
    }
}