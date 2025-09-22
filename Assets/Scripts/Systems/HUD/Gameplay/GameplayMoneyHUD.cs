using UnityEngine;
using TMPro;
using System;

public class GameplayMoneyHUD : MonoBehaviour
{
    [Header("Settings")]
    public string PrefixText = "<size=50%>Money:</size>\n<b>";
    public string PostfixText = "</b>";

    [Header("References")]
    public GameplayManager GameplayManager;
    public GameplayMoney GameplayMoney;
    public CanvasGroup CanvasGroup;
    public TMP_Text Text;

    private void Start()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);

        GameplayManager.OnStageStart += GameplayStageStart;
        GameplayManager.OnStageEnd += GameplayStageEnd;

        GameplayMoney.OnChange.AddListener(GameplayMoneyChange);
    }

    private void OnDestroy()
    {
        GameplayManager.OnStageStart -= GameplayStageStart;
        GameplayManager.OnStageEnd -= GameplayStageEnd;

        GameplayMoney.OnChange.RemoveListener(GameplayMoneyChange);
    }

    public void Show()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, true);
        UpdateText();
    }

    public void Hide()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);
    }

    private void GameplayStageStart()
    {
        Show();
    }

    private void GameplayStageEnd(StageEndReason reason)
    {
        Hide();
    }

    private void GameplayMoneyChange()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        Text.text = PrefixText + GameplayMoney.TotalMoney.ToString() + PostfixText;
    }
}