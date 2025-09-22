using System;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Conditions/WithinRangeTarget", fileName = "WithinRangeTargetCondition")]
    public class WithinRangeTargetCondition : ConditionAsset
    {
        [Header("Settings")]
        [SerializeField]
        private float MinDistance;

        public override bool Evaluate(IFiniteStateMachineStorage storage)
        {
            AIAgent agent = storage.Read<AIAgent>();
            if (!agent.Target.IsValid)
            {
                return false;
            }

            Vector3 source = agent.transform.position;
            Vector3 targetDiff = agent.Target.Position - source;
            float dist = targetDiff.magnitude;
            return dist <= MinDistance;
        }
    }
}