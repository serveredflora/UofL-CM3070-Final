using UnityEngine;

namespace FSM
{
    public abstract class ConditionAsset : ScriptableObject, ICondition
    {
        public abstract bool Evaluate(IFiniteStateMachineStorage storage);
    }
}