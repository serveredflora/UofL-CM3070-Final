using System;
using UnityEngine;
using PrimeTween;
using System.Collections.Generic;
using UnityEngine.UI;
using TriInspector;
using System.Linq;

public class PlayerInteractionHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerInteraction PlayerInteraction;

    [Space(10)]
    [SerializeField]
    private CanvasGroup OriginCanvasGroup;
    [SerializeField]
    private RectTransform OriginTransform;

    [Space(10)]
    [SerializeField]
    private CanvasGroup PanelCanvasGroup;
    [SerializeField]
    private RectTransform PanelTransform;

    [Space(10)]
    [SerializeField]
    private TMPro.TMP_Text ActionInfoText;
    [SerializeField]
    private TMPro.TMP_Text ActionFailText;

    [Space(10)]
    [SerializeField]
    private CanvasGroup PaginationCanvasGroup;
    [SerializeField]
    private RectTransform PaginationTransform;
    [SerializeField]
    [AssetsOnly]
    private Image PaginationPrefab;

    private Sequence? RunningSequence = null;

    private readonly List<Image> UsedPaginationImages = new();
    private readonly List<Image> UnusedPaginationImages = new();

    private void Start()
    {
        PlayerInteraction.OnFocusInteractable += FocusedInteractable;
        PlayerInteraction.OnUnfocusInteractable += UnfocusedInteractable;
        PlayerInteraction.OnCycleToAction += CycledToAction;
        PlayerInteraction.OnExecuteAction += ExecutedAction;

        OriginTransform.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerInteraction.OnFocusInteractable -= FocusedInteractable;
        PlayerInteraction.OnUnfocusInteractable -= UnfocusedInteractable;
        PlayerInteraction.OnCycleToAction -= CycledToAction;
        PlayerInteraction.OnExecuteAction -= ExecutedAction;
    }

    private void Show(IPlayerInteractable interactable, IPlayerInteractableAction cycledAction)
    {
        if (RunningSequence.HasValue)
        {
            RunningSequence.Value.Stop();
        }

        SetPaginationCount(interactable.Actions.Count);
        CycledToAction(interactable, cycledAction);

        OriginCanvasGroup.alpha = 0.0f;

        OriginTransform.gameObject.SetActive(true);
        OriginTransform.localEulerAngles = Vector3.zero;
        OriginTransform.localScale = new Vector3(1.75f, 0.6f, 1.0f);

        RunningSequence = Sequence.Create()
            .Group(Tween.Alpha(OriginCanvasGroup, endValue: 1.0f, duration: 0.15f, ease: Ease.OutQuint))
            .Group(Tween.Scale(OriginTransform, endValue: Vector3.one, duration: 0.25f, ease: Ease.OutQuint))
            .OnComplete(() =>
            {
                RunningSequence = null;
            });
    }

    private void Close()
    {
        if (RunningSequence.HasValue)
        {
            RunningSequence.Value.Stop();
        }

        RunningSequence = Sequence.Create()
           .Group(Tween.Alpha(OriginCanvasGroup, endValue: 0.0f, duration: 0.15f, ease: Ease.OutQuint))
           .Group(Tween.Scale(OriginTransform, endValue: new Vector3(2.0f, 0.75f, 1.0f), duration: 0.25f, ease: Ease.OutQuint))
           .OnComplete(() =>
           {
               RunningSequence = null;
               OriginTransform.gameObject.SetActive(false);
           });
    }

    private void FocusedInteractable(IPlayerInteractable interactable, IPlayerInteractableAction action)
    {
        Show(interactable, action);
    }

    private void UnfocusedInteractable(IPlayerInteractable interactable, IPlayerInteractableAction action)
    {
        Close();
    }

    private void CycledToAction(IPlayerInteractable interactable, IPlayerInteractableAction action)
    {
        int index = interactable.Actions.ToList().IndexOf(action);
        SetPaginationIndex(index);

        SetAction(action);
    }

    private void SetAction(IPlayerInteractableAction action)
    {
        ActionInfoText.text = action.Info;

        ActionFailText.color = Color.clear;
        ActionFailText.transform.localScale = Vector3.zero;
    }

    private void ExecutedAction(IPlayerInteractable interactable, IPlayerInteractableAction action, PlayerInteractableActionPerformResult result)
    {
        if (RunningSequence.HasValue)
        {
            RunningSequence.Value.Stop();
        }

        if (!result.Success)
        {
            float startAngle = UnityEngine.Random.Range(5.0f, 8.5f);
            if (UnityEngine.Random.value >= 0.5f)
            {
                startAngle = -startAngle;
            }

            OriginTransform.localEulerAngles = new Vector3(0.0f, 0.0f, startAngle);

            ActionFailText.color = Color.red;
            ActionFailText.transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            ActionFailText.color = Color.clear;
        }

        OriginCanvasGroup.alpha = 1.0f;
        OriginTransform.localScale = Vector3.one * 1.15f;

        RunningSequence = Sequence.Create()
           .Group(Tween.LocalRotation(OriginTransform, endValue: Vector3.zero, duration: 0.15f, ease: Ease.OutQuint))
           .Group(Tween.Scale(OriginTransform, endValue: Vector3.one, duration: 0.15f, ease: Ease.OutQuint))
           .Group(Tween.Scale(ActionFailText.transform, endValue: Vector3.one, duration: 0.15f, ease: Ease.OutQuint))
           .Group(Tween.Color(ActionFailText, endValue: ActionFailText.color.WithA(0.0f), duration: 1.0f, startDelay: 0.3f, ease: Ease.InQuint))
           .Group(Tween.Scale(ActionFailText.transform, endValue: Vector3.zero, duration: 1.0f, startDelay: 0.3f, ease: Ease.InQuint))
           .OnComplete(() =>
           {
               RunningSequence = null;
           });
    }

    private void SetPaginationCount(int count)
    {
        // TODO: move the following code into something reusable
        if (UsedPaginationImages.Count > count)
        {
            while (UsedPaginationImages.Count > count)
            {
                var lastIndex = UsedPaginationImages.Count - 1;
                var paginationImage = UsedPaginationImages[lastIndex];
                UsedPaginationImages.RemoveAt(lastIndex);

                paginationImage.gameObject.SetActive(false);

                UnusedPaginationImages.Add(paginationImage);
            }
        }
        else if (UsedPaginationImages.Count < count)
        {
            while (UnusedPaginationImages.Count > 0 && UsedPaginationImages.Count < count)
            {
                var lastIndex = UnusedPaginationImages.Count - 1;
                var paginationImage = UnusedPaginationImages[lastIndex];
                UnusedPaginationImages.RemoveAt(lastIndex);

                paginationImage.gameObject.SetActive(true);

                UsedPaginationImages.Add(paginationImage);
            }

            while (UsedPaginationImages.Count < count)
            {
                var paginationImage = Instantiate(PaginationPrefab, PaginationTransform);
                UsedPaginationImages.Add(paginationImage);
            }
        }

        PaginationCanvasGroup.alpha = count > 1 ? 1.0f : 0.0f;
    }

    private void SetPaginationIndex(int index)
    {
        for (int i = 0; i < UsedPaginationImages.Count; ++i)
        {
            UsedPaginationImages[i].color = UsedPaginationImages[i].color.WithA(i == index ? 1.0f : 0.1f);
        }
    }
}