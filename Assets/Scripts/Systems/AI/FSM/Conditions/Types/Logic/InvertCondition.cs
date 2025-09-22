using TriInspector;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Conditions/Logic/Invert", fileName = "InvertCondition")]
    public class InvertCondition : ConditionAsset
    {
        [InlineEditor]
        [SerializeField]
        private ConditionAsset InternalCondition;

        public override bool Evaluate(IFiniteStateMachineStorage storage)
        {
            return !InternalCondition.Evaluate(storage);
        }
    }
}