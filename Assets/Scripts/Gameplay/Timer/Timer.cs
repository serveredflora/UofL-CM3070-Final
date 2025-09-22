using System;

public class Timer : ITimer
{
    public event Action<float> OnTimeout;

    public bool Paused { get; set; }
    public bool OneShot { get; set; }

    public bool IsRunning { get; private set; }

    public float WaitTime { get; private set; }
    public float TimeLeft { get; private set; }

    public void Begin(float waitTime, bool restart = false)
    {
        if (IsRunning && !restart)
        {
            return;
        }

        WaitTime = waitTime;
        TimeLeft = WaitTime;
        IsRunning = true;
    }

    public void End()
    {
        IsRunning = false;
    }

    public void Tick(float deltaTime)
    {
        if (!IsRunning || Paused)
        {
            return;
        }

        TimeLeft -= deltaTime;
        if (TimeLeft > 0.0f)
        {
            return;
        }

        float overflow = -TimeLeft;
        if (OneShot)
        {
            OnTimeout?.Invoke(overflow);
            IsRunning = false;
        }
        else
        {
            int timeoutIdx = 0;
            while (TimeLeft < 0.0f)
            {
                TimeLeft += WaitTime;
                OnTimeout?.Invoke(overflow - (timeoutIdx * WaitTime));
                ++timeoutIdx;
            }
        }
    }
}