using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsHUD : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Color DamageFlashColor;
    [SerializeField]
    private float DamageFlashDuration;
    [SerializeField]
    private Ease DamageFlashEase;

    [Header("References")]
    [SerializeField]
    private PlayerStats PlayerStats;
    [Space(10)]
    [SerializeField]
    private SegmentedProgressBar HealthProgressBar;
    [SerializeField]
    private Image DamageFlashOverlay;

    private Tween? DamageFlashTween = null;

    private void Start()
    {
        PlayerStats.Health.OnChangeInfo.AddListener(PlayerHealthChangeInfo);

        UpdateHealthProgressBar();
        DamageFlashOverlay.color = DamageFlashColor.WithA(0.0f);
    }

    private void OnDestroy()
    {
        PlayerStats.Health.OnChangeInfo.RemoveListener(PlayerHealthChangeInfo);
    }

    private void PlayerHealthChangeInfo(int previous, int current)
    {
        UpdateHealthProgressBar();
        UpdateDamageFlash(previous, current);
    }

    private void UpdateHealthProgressBar()
    {
        HealthProgressBar.Value = PlayerStats.Health.CurrentHealth;
        HealthProgressBar.Range = PlayerStats.Health.MaxHealth;
    }

    private void UpdateDamageFlash(int previous, int current)
    {
        if (previous <= current)
        {
            return;
        }

        if (DamageFlashTween.HasValue)
        {
            DamageFlashTween.Value.Stop();
        }

        DamageFlashTween = Tween.Color(
            DamageFlashOverlay,
            startValue: DamageFlashColor,
            endValue: DamageFlashColor.WithA(0.0f),
            duration: DamageFlashDuration,
            ease: DamageFlashEase
        );
    }
}
