using System;
using System.Collections;
using PrimeTween;
using UnityEngine;

public class PlayerHotbarVisualization : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Vector3 EquipPosition;
    [SerializeField]
    private Vector3 UnequipPosition;
    [Space(10)]
    [SerializeField]
    private float EquipDuration = 0.4f;
    [SerializeField]
    private float UnequipDuration = 0.15f;

    [Space(10)]
    [SerializeField]
    private float RequeueActionDuration = 0.2f;

    [Header("References")]
    [SerializeField]
    private PlayerHotbar PlayerHotbar;
    [SerializeField]
    private Animator Animator;

    private Item QueuedEquip = null;
    private bool QueuedUnequip = false;

    private Sequence? CurrentSequence = null;

    private bool QueuePrimaryAction;
    private bool QueueSecondaryAction;

    private int CurrentAnimation = 0;
    private ITimer CurrentAnimationTimer;

    private static readonly int IdleAnimation = Animator.StringToHash("PlayerHeldItemIdle");
    private static readonly int SwingAnimation = Animator.StringToHash("PlayerHeldItemSwing");

    private void OnEnable()
    {
        CurrentAnimationTimer = new Timer();
        CurrentAnimationTimer.OneShot = true;

        PlayerHotbar.OnSlotSelect += HotbarSlotSelected;
        PlayerHotbar.OnSlotDeselect += HotbarSlotDeselected;

        PlayerHotbar.OnPerformPrimaryAction += HotbarPerformPrimaryAction;
        PlayerHotbar.OnPerformSecondaryAction += HotbarPerformSecondaryAction;

        ChangeAnimation(IdleAnimation, 0.0f);
    }

    private void OnDisable()
    {
        PlayerHotbar.OnSlotSelect -= HotbarSlotSelected;
        PlayerHotbar.OnSlotDeselect -= HotbarSlotDeselected;

        PlayerHotbar.OnPerformPrimaryAction -= HotbarPerformPrimaryAction;
        PlayerHotbar.OnPerformSecondaryAction -= HotbarPerformSecondaryAction;
    }

    private void Update()
    {
        UpdateEquipAnimation();
        UpdateActionAnimation();
    }

    private void UpdateEquipAnimation()
    {
        if (CurrentSequence != null)
        {
            // Currently playing sequence
            return;
        }

        if (QueuedUnequip)
        {
            QueuedUnequip = false;
            CurrentSequence = Sequence.Create()
                .Group(Tween.LocalPosition(transform, startValue: EquipPosition, endValue: UnequipPosition, duration: UnequipDuration, ease: Ease.OutQuint))
                .OnComplete(() =>
                {
                    CurrentSequence = null;
                });
        }
        else if (QueuedEquip != null)
        {
            QueuedEquip = null;
            CurrentSequence = Sequence.Create()
                .Group(Tween.LocalPosition(transform, startValue: UnequipPosition, endValue: EquipPosition, duration: EquipDuration, ease: Ease.OutQuint))
                .OnComplete(() =>
                {
                    CurrentSequence = null;
                });
        }
    }

    private void UpdateActionAnimation()
    {
        CurrentAnimationTimer.Tick(Time.deltaTime);

        if (CurrentAnimation == IdleAnimation)
        {
            if (QueuePrimaryAction)
            {
                ChangeAnimation(SwingAnimation, 0.05f);
                QueuePrimaryAction = false;
            }
        }
    }

    public void ChangeAnimation(int animation, float crossFade = 0.1f, float queueTime = 0.0f)
    {
        if (queueTime > 0.0f)
        {
            CurrentAnimationTimer.Begin(queueTime);
            StartCoroutine(Wait());
        }
        else
        {
            Perform();
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(queueTime);
            Perform();
        }

        void Perform()
        {
            if (animation != CurrentAnimation)
            {
                Animator.CrossFade(animation, crossFade);
                CurrentAnimation = animation;
            }
        }
    }

    private void HotbarSlotSelected(int index)
    {
        // QueuedEquip = PlayerHotbar.Items[index];
        QueuedEquip = new Item(null);
    }

    private void HotbarSlotDeselected(int index)
    {
        QueuedUnequip = true;
    }

    private void HotbarPerformPrimaryAction()
    {
        if (CurrentAnimationTimer.IsRunning && CurrentAnimationTimer.TimeLeft > RequeueActionDuration)
        {
            return;
        }

        QueuePrimaryAction = true;
    }

    private void HotbarPerformSecondaryAction()
    {
        if (CurrentAnimationTimer.IsRunning && CurrentAnimationTimer.TimeLeft > RequeueActionDuration)
        {
            return;
        }

        QueueSecondaryAction = true;
    }
}