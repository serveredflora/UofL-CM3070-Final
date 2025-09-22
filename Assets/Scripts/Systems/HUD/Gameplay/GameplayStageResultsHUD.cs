using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayStageResultsHUD : MonoBehaviour
{
    [Header("References")]
    public GameplayManager GameplayManager;
    public GameplayStageStartHUD GameplayStageStartHUD;

    [Space(10)]
    public CanvasGroup CanvasGroup;
    public TMP_Text ReasonText;
    public Button EndRunButton;
    public Button NextStageButton;

    private void Start()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);

        GameplayManager.OnRunStart += GameplayRunStart;
        GameplayManager.OnRunEnd += GameplayRunEnd;
        GameplayManager.OnStageStart += GameplayStageStart;
        GameplayManager.OnStageEnd += GameplayStageEnd;

        EndRunButton.onClick.AddListener(EndRunButtonClick);
        NextStageButton.onClick.AddListener(NextStageButtonClick);
    }

    private void OnDestroy()
    {
        GameplayManager.OnRunStart -= GameplayRunStart;
        GameplayManager.OnRunEnd -= GameplayRunEnd;
        GameplayManager.OnStageStart -= GameplayStageStart;
        GameplayManager.OnStageEnd -= GameplayStageEnd;

        EndRunButton.onClick.RemoveListener(EndRunButtonClick);
        NextStageButton.onClick.RemoveListener(NextStageButtonClick);
    }

    public void Show(StageEndReason reason)
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, true);
        UpdateReasonText(reason);
        UpdateButtonState(reason);
    }

    public void Hide()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);
    }

    private void UpdateReasonText(StageEndReason reason)
    {
        ReasonText.text = $"Reason: " + reason.ToString();
    }

    private void UpdateButtonState(StageEndReason reason)
    {
        bool canContinue = reason == StageEndReason.ReachExit;

        EndRunButton.gameObject.SetActive(!canContinue);
        NextStageButton.gameObject.SetActive(canContinue);
    }

    private void GameplayRunStart()
    {
        Hide();
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
        Show(reason);
    }

    private void EndRunButtonClick()
    {
        GameplayManager.EndRun();
    }

    private void NextStageButtonClick()
    {
        Hide();
        GameplayStageStartHUD.Show();
    }
}
