using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

    public class FiniteStateMachine
    {
        public readonly IState InitialState;
        public IState CurrentState { get; private set; }

        private IFiniteStateMachineStorage Storage;

        public FiniteStateMachine(IFiniteStateMachineStorage storage, IState initialState)
        {
            Storage = storage;

            InitialState = initialState;
            CurrentState = InitialState;
        }

        public void Tick()
        {
            ITransition triggeredTransition = null;
            foreach (ITransition t in CurrentState.Transitions)
            {
                if (t.IsTriggered(Storage))
                {
                    triggeredTransition = t;
                    break;
                }
            }

            List<IAction> actions = new List<IAction>();
            if (triggeredTransition != null)
            {
                IState targetState = triggeredTransition.TargetState;
                actions.Add(CurrentState.ExitAction);
                actions.Add(triggeredTransition.Action);
                actions.Add(targetState.EntryAction);
                CurrentState = targetState;
            }

            foreach (IAction action in CurrentState.Actions)
            {
                actions.Add(action);
            }

            DoActions(actions);
        }

        private void DoActions(List<IAction> actions)
        {
            foreach (IAction action in actions)
            {
                if (action != null)
                {
                    action.Perform(Storage);
                }
            }
        }
    }
}