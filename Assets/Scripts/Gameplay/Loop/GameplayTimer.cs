using System;
using UnityEngine;
using UnityEngine.Events;

public class GameplayTimer : MonoBehaviour
{
    public UnityEvent OnTimeout;

    public ITimerReadOnly TimerReadOnly => Timer;
    private ITimer Timer;

    private void Awake()
    {
        Timer = new Timer();
        Timer.OneShot = true;
        Timer.OnTimeout += TimerTimeout;
    }

    public void Begin(float duration)
    {
        Timer.Begin(duration, true);
    }

    public void End()
    {
        Timer?.End();
    }

    private void Update()
    {
        Timer?.Tick(Time.deltaTime);
    }

    private void TimerTimeout(float overflow)
    {
        OnTimeout?.Invoke();
    }
}
