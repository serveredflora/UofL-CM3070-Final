using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayRunStartHUD : MonoBehaviour
{
    [Header("References")]
    public GameplayManager GameplayManager;
    public CanvasGroup CanvasGroup;
    public Button StartRunButton;


    private void Start()
    {
        GameplayManager.OnRunStart += GameplayRunStart;
        GameplayManager.OnRunEnd += GameplayRunEnd;
        GameplayManager.OnStageStart += GameplayStageStart;
        GameplayManager.OnStageEnd += GameplayStageEnd;

        StartRunButton.onClick.AddListener(NewRunButtonClick);
    }

    private void OnDestroy()
    {
        GameplayManager.OnRunStart -= GameplayRunStart;
        GameplayManager.OnRunEnd -= GameplayRunEnd;
        GameplayManager.OnStageStart -= GameplayStageStart;
        GameplayManager.OnStageEnd -= GameplayStageEnd;

        StartRunButton.onClick.RemoveListener(NewRunButtonClick);
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
        Hide();
    }

    private void GameplayRunEnd()
    {
        // Show();
    }

    private void GameplayStageStart()
    {
        Hide();
    }

    private void GameplayStageEnd(StageEndReason reason)
    {
        Hide();
    }

    private void NewRunButtonClick()
    {
        Hide();

        // TODO: get config from a class that does this
        GameplayManager.StartRun(new RunConfig() { Seed = 1000 });
    }
}
