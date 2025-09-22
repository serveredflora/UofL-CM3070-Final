using System;

public interface ITimerReadOnly
{
    event Action<float> OnTimeout;

    float WaitTime { get; }
    float TimeLeft { get; }

    bool IsRunning { get; }
}
