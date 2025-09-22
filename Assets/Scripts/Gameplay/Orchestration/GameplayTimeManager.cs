using System;
using UnityEngine;

public class GameplayTimeManager : MonoBehaviour
{
    public event Action OnPause;
    public event Action OnResume;

    public float TimeScale { get; private set; } = 1.0f;
    public bool IsPaused { get; private set; }

    public void TogglePause()
    {
        if (!IsPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        if (IsPaused)
        {
            return;
        }

        Time.timeScale = 0.0f;

        IsPaused = true;
        OnPause?.Invoke();
    }

    public void Resume()
    {
        if (!IsPaused)
        {
            return;
        }

        Time.timeScale = TimeScale;

        IsPaused = false;
        OnResume?.Invoke();
    }

    public void SetTimeScale(float value)
    {
        TimeScale = value;

        if (!IsPaused)
        {
            Time.timeScale = value;
        }
    }
}
