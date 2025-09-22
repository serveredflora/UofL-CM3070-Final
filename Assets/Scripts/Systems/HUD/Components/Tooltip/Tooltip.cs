using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Tooltip : MonoBehaviour
{
    private bool IsShown = false;

    private const float TweenSpeed = 12.0f;

    private void LateUpdate()
    {
        if (!IsShown)
        {
            return;
        }

        UpdatePosition(instant: false);
    }

    private void UpdatePosition(bool instant)
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector3 goalPosition = new Vector3(mousePos.x, mousePos.y, 0.0f);

        if (instant)
        {
            transform.position = goalPosition;
            return;
        }

        transform.position += (goalPosition - transform.position) * (1.0f - Mathf.Exp(-TweenSpeed * Time.deltaTime));
    }

    public void Show()
    {
        if (IsShown)
        {
            return;
        }

        if (!TryShowInternal())
        {
            return;
        }

        transform.localScale = Vector3.zero;
        Tween.Scale(transform, endValue: Vector3.one, duration: 0.25f, ease: Ease.OutQuint);

        UpdatePosition(instant: true);

        IsShown = true;
    }

    public void Hide()
    {
        if (!IsShown)
        {
            return;
        }

        if (!TryHideInternal())
        {
            return;
        }

        IsShown = false;
    }


    protected abstract bool TryShowInternal();
    protected abstract bool TryHideInternal();
}