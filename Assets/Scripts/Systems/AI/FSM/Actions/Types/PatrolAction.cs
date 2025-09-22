using System;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Patrol", fileName = "PatrolAction")]
    public class PatrolAction : ActionAsset
    {
        public override void Perform(IFiniteStateMachineStorage storage)
        {
            AIAgent agent = storage.Read<AIAgent>();
            if (agent.IsAtDestination())
            {
                agent.GoToNextWaypoint();
            }
        }
    }
}