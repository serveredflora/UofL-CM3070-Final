using UnityEngine;

public static class HUDUtils
{
    public static void SetCanvasGroupState(CanvasGroup canvasGroup, bool value)
    {
        canvasGroup.alpha = value ? 1.0f : 0.0f;
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }
}