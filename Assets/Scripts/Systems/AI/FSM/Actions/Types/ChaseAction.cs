using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Chase", fileName = "ChaseAction")]
    public class ChaseAction : ActionAsset
    {
        public override void Perform(IFiniteStateMachineStorage storage)
        {
            AIAgent agent = storage.Read<AIAgent>();
            agent.GoToTarget();
        }
    }
}