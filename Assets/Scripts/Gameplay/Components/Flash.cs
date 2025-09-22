using PrimeTween;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [Header("Settings")]
    public Color Color = Color.white;
    public float Delay = 0.05f;
    public float Duration = 0.2f;

    [Header("References")]
    public MeshRenderer[] MeshRenderers;

    private static readonly int _OverrideColorPropertyId = Shader.PropertyToID("_OverrideColor");
    private Tween? _CurrentTween;
    private MaterialPropertyBlock _PropertyBlock;

    private void Start()
    {
        _PropertyBlock = new();
    }

    private void OnDestroy()
    {
        if (_CurrentTween.HasValue)
        {
            _CurrentTween.Value.Stop();
        }
    }

    public void Trigger()
    {
        if (_CurrentTween.HasValue)
        {
            _CurrentTween.Value.Stop();
            _CurrentTween = null;
        }

        SetColor(Color);

        var tweenSettings = new TweenSettings(startDelay: Delay, duration: Duration, updateType: UpdateType.Update, ease: Ease.OutQuint);
        _CurrentTween = Tween.Custom(startValue: Color, endValue: Color.WithA(0.0f), settings: tweenSettings, onValueChange: SetColor);
    }

    private void SetColor(Color color)
    {
        _PropertyBlock.SetColor(_OverrideColorPropertyId, color);
        foreach (var meshRenderer in MeshRenderers)
        {
            meshRenderer.SetPropertyBlock(_PropertyBlock);
        }
    }
}