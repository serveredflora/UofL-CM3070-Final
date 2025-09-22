using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StageExitZone : MonoBehaviour
{
    public event Action OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerCharacter playerCharacter))
        {
            return;
        }

        OnTrigger?.Invoke();
    }
}