using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Attack", fileName = "AttackAction")]
    public class AttackAction : ActionAsset
    {
        public override void Perform(IFiniteStateMachineStorage storage)
        {
            AIAgent agent = storage.Read<AIAgent>();
            agent.StopMoving();
            agent.PerformAttack();
        }
    }
}