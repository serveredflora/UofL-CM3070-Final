using TriInspector;
using UnityEngine;

namespace FSM
{
    public abstract class ActionAsset : ScriptableObject, IAction
    {
        public abstract void Perform(IFiniteStateMachineStorage storage);
    }
}