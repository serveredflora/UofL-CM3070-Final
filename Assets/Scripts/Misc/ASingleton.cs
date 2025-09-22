using System;
using UnityEngine;

public abstract class ASingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Start()
    {
        if (Instance != null)
        {
            throw new InvalidOperationException($"Multiple instances of {typeof(T)} exist");
        }

        Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (Instance == null)
        {
            throw new InvalidOperationException($"Multiple instances of {typeof(T)} exist");
        }

        Instance = null;
    }
}
