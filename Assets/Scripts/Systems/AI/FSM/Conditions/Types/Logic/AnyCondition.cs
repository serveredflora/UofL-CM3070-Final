using System.Linq;
using TriInspector;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Conditions/Logic/Any", fileName = "AnyCondition")]
    public class AnyCondition : ConditionAsset
    {
        [InlineEditor]
        [SerializeField]
        private ConditionAsset[] InternalConditions;

        public override bool Evaluate(IFiniteStateMachineStorage storage)
        {
            return InternalConditions.Any(e => e.Evaluate(storage));
        }
    }
}