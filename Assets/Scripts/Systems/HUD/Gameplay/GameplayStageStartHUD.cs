using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayStageStartHUD : MonoBehaviour
{
    [Header("References")]
    public GameplayManager GameplayManager;
    public CanvasGroup CanvasGroup;
    public Button StartStageButton;


    private void Start()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);

        GameplayManager.OnRunStart += GameplayRunStart;
        GameplayManager.OnRunEnd += GameplayRunEnd;
        GameplayManager.OnStageStart += GameplayStageStart;
        GameplayManager.OnStageEnd += GameplayStageEnd;

        StartStageButton.onClick.AddListener(StartStageButtonClick);
    }

    private void OnDestroy()
    {
        GameplayManager.OnRunStart -= GameplayRunStart;
        GameplayManager.OnRunEnd -= GameplayRunEnd;
        GameplayManager.OnStageStart -= GameplayStageStart;
        GameplayManager.OnStageEnd -= GameplayStageEnd;

        StartStageButton.onClick.RemoveListener(StartStageButtonClick);
    }

    public void Show()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, true);
    }

    public void Hide()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);
    }

    private void GameplayRunStart()
    {
        Show();
    }

    private void GameplayRunEnd()
    {
        Hide();
    }

    private void GameplayStageStart()
    {
        Hide();
    }

    private void GameplayStageEnd(StageEndReason reason)
    {
        Hide();
    }

    private void StartStageButtonClick()
    {
        Hide();

        // TODO: get config from a class that does this
        GameplayManager.StartStage(new StageConfig() { Duration = 120.0f });
    }
}
