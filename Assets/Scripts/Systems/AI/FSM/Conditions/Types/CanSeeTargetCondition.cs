using System;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Conditions/CanSeeTarget", fileName = "CanSeeTargetCondition")]
    public class CanSeeTargetCondition : ConditionAsset
    {
        [Header("Settings")]
        [SerializeField]
        private Vector3 ViewOriginOffset;
        [SerializeField]
        private float ViewAngle;
        [SerializeField]
        private float ViewDistance;

        private RaycastHit[] RaycastResults = new RaycastHit[8];

        public override bool Evaluate(IFiniteStateMachineStorage storage)
        {
            AIAgent agent = storage.Read<AIAgent>();
            if (!agent.Target.IsValid)
            {
                return false;
            }

            Vector3 source = agent.transform.position + ViewOriginOffset;
            Vector3 targetDiff = agent.Target.Position - source;
            float dist = targetDiff.magnitude;
            if (dist > ViewDistance)
            {
                return false;
            }

            Vector3 targetDir = targetDiff.normalized;
            float angle = Vector3.Angle(targetDir, agent.transform.forward);
            if (angle > ViewAngle)
            {
                return false;
            }

            var ray = new Ray(source, targetDir);
            int hitCount = PhysicsUtils.SortedRaycast(ray, RaycastResults, maxDistance: ViewDistance, triggerOpt: QueryTriggerInteraction.Collide);

            bool isTargetUnobstructed = true;
            for (int hitIndex = 0; hitIndex < hitCount; ++hitIndex)
            {
                RaycastHit hit = RaycastResults[hitIndex];
                if (hit.collider == null)
                {
                    continue;
                }

                if (hit.collider == agent.Target.Collider)
                {
                    break;
                }

                if (hit.collider.isTrigger)
                {
                    continue;
                }

                if (hit.collider is CharacterController)
                {
                    continue;
                }

                isTargetUnobstructed = false;
                break;
            }

            if (!isTargetUnobstructed)
            {
                return false;
            }

            return true;
        }
    }
}