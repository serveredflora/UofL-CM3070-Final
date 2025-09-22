using TriInspector;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Transition", fileName = "Transition")]
    public class TransitionAsset : ScriptableObject, ITransition
    {
        [InlineEditor]
        [SerializeField]
        private ConditionAsset MetaDecision;
        [InlineEditor]
        [SerializeField]
        private StateAsset MetaTargetState;
        [InlineEditor]
        [SerializeField]
        private ActionAsset MetaAction;

        public ICondition Decision => MetaDecision;

        public IState TargetState => MetaTargetState;

        public IAction Action => MetaAction;
    }
}