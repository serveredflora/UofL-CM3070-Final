using UnityEngine;

public class PlayerHotbarVisualizationAnimationOnFinish : StateMachineBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private AnimationClip EndAnimationClip;
    [SerializeField]
    private float CrossFade = 0.1f;

    private int EndAnimation => Animator.StringToHash(EndAnimationClip.name);

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var visualization = animator.GetComponent<PlayerHotbarVisualization>();
        visualization.ChangeAnimation(EndAnimation, CrossFade, stateInfo.length);
    }
}
