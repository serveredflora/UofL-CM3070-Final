using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/State", fileName = "State")]
    public class StateAsset : ScriptableObject, IState
    {
        [InlineEditor]
        [SerializeField]
        private ActionAsset[] MetaActions;
        [InlineEditor]
        [SerializeField]
        private ActionAsset MetaEntryAction;
        [InlineEditor]
        [SerializeField]
        private ActionAsset MetaExitAction;
        [InlineEditor]
        [SerializeField]
        private TransitionAsset[] MetaTransitions;

        // Interface implementation
        public IReadOnlyList<IAction> Actions => MetaActions;
        public IAction EntryAction => MetaEntryAction;
        public IAction ExitAction => MetaExitAction;
        public IReadOnlyList<ITransition> Transitions => MetaTransitions;
    }
}