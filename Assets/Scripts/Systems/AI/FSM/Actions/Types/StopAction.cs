using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Stop", fileName = "StopAction")]
    public class StopAction : ActionAsset
    {
        public override void Perform(IFiniteStateMachineStorage storage)
        {
            AIAgent agent = storage.Read<AIAgent>();
            agent.StopMoving();
        }
    }
}