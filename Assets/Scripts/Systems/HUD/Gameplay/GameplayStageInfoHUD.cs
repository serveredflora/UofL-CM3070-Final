using UnityEngine;
using TMPro;

public class GameplayStageInfoHUD : MonoBehaviour
{
    [Header("Settings")]
    public string PrefixText = "<size=50%>Money:</size>\n<b>";
    public string PostfixText = "</b>";

    [Header("References")]
    public GameplayManager GameplayManager;
    public CanvasGroup CanvasGroup;
    public TMP_Text Text;

    private void Start()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);

        GameplayManager.OnStageStart += GameplayStageStart;
        GameplayManager.OnStageEnd += GameplayStageEnd;
    }

    private void OnDestroy()
    {
        GameplayManager.OnStageStart -= GameplayStageStart;
        GameplayManager.OnStageEnd -= GameplayStageEnd;
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

    private void UpdateText()
    {
        Text.text = PrefixText + GameplayManager.CurrentStageNumber.ToString() + PostfixText;
    }
}
