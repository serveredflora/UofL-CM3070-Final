using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class WorldButtonUnityEventPlayerInteractableAction : IWorldButtonPlayerInteractableAction
{
    [Header("Settings")]
    [SerializeField]
    private string _Info;

    [Space(10)]
    [SerializeField]
    private UnityEvent OnPerform;

    public string Info => _Info;

    public PlayerInteractableActionPerformResult Perform(Player player)
    {
        OnPerform?.Invoke();
        return new PlayerInteractableActionPerformResult(true);
    }
}
