using UnityEngine;
using TMPro;
using System;

public class GameplayTimerHUD : MonoBehaviour
{
    [Header("Settings")]
    public string PrefixText = "<size=50%>Return in:</size>\n<b>";
    public string PostfixText = "</b>";

    [Header("References")]
    public GameplayManager GameplayManager;
    public GameplayTimer GameplayTimer;
    public CanvasGroup CanvasGroup;
    public TMP_Text Text;

    private ITimerReadOnly Timer;

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

        if (Timer != null)
        {
            Timer.OnTimeout -= TimerTimeout;
        }
    }

    private void Update()
    {
        if (Timer == null || !Timer.IsRunning)
        {
            return;
        }

        // TODO: add tweening for near end of timer to grab player's attention + increase intensity
        UpdateText();
    }

    private void GameplayStageStart()
    {
        Timer = GameplayTimer.TimerReadOnly;
        Timer.OnTimeout += TimerTimeout;

        HUDUtils.SetCanvasGroupState(CanvasGroup, true);
        UpdateText();
    }

    private void GameplayStageEnd(StageEndReason reason)
    {
        Timer.OnTimeout -= TimerTimeout;
        Timer = null;

        HUDUtils.SetCanvasGroupState(CanvasGroup, false);
    }

    private void UpdateText()
    {
        Text.text = GetText(Timer.TimeLeft);
    }

    private string GetText(float timeLeft)
    {
        int mins = ((int)Math.Floor(timeLeft)) / 60;
        int seconds = ((int)Math.Floor(timeLeft)) % 60;
        int milliseconds = (int)Math.Floor((timeLeft - (float)Math.Floor(timeLeft)) * 1000);

        string minsStr = $"{mins:D1}m";
        string secondsStr = $"{seconds:D2}s";
        string millisecondsStr = $"{milliseconds:D3}ms";

        string[] timeStrs = mins > 0 ? new string[] { minsStr, secondsStr } : new string[] {
            secondsStr, millisecondsStr
        };

        return PrefixText + string.Join(':', timeStrs) + PostfixText;
    }

    private void TimerTimeout(float overflow)
    {
        Text.text = GetText(0.0f);
    }
}