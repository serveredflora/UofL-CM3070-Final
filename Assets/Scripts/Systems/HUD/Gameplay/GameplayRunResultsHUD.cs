using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayRunResultsHUD : MonoBehaviour
{
    [Header("References")]
    public GameplayManager GameplayManager;
    public GameplayRunStartHUD GameplayRunStartHUD;

    [Space(10)]
    public CanvasGroup CanvasGroup;
    public Button NewRunButton;

    private void Start()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);

        GameplayManager.OnRunStart += GameplayRunStart;
        GameplayManager.OnRunEnd += GameplayRunEnd;
        GameplayManager.OnStageStart += GameplayStageStart;
        GameplayManager.OnStageEnd += GameplayStageEnd;

        NewRunButton.onClick.AddListener(NewRunButtonClick);
    }

    private void OnDestroy()
    {
        GameplayManager.OnRunStart -= GameplayRunStart;
        GameplayManager.OnRunEnd -= GameplayRunEnd;
        GameplayManager.OnStageStart -= GameplayStageStart;
        GameplayManager.OnStageEnd -= GameplayStageEnd;

        NewRunButton.onClick.RemoveListener(NewRunButtonClick);
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
        Show();
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
        GameplayRunStartHUD.Show();
    }
}
