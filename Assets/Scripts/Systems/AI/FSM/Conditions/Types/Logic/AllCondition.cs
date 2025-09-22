using System.Linq;
using TriInspector;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Conditions/Logic/All", fileName = "AllCondition")]
    public class AllCondition : ConditionAsset
    {
        [InlineEditor]
        [SerializeField]
        private ConditionAsset[] InternalConditions;

        public override bool Evaluate(IFiniteStateMachineStorage storage)
        {
            return InternalConditions.All(e => e.Evaluate(storage));
        }
    }
}