using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(VisualElement))]
public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnEnter;
    public event Action OnExit;

    private bool IsEntered = false;

    private void OnDisable()
    {
        if (!IsEntered)
        {
            return;
        }

        OnExit?.Invoke();
        IsEntered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEntered)
        {
            return;
        }

        OnEnter?.Invoke();
        IsEntered = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsEntered)
        {
            return;
        }

        OnExit?.Invoke();
        IsEntered = false;
    }
}