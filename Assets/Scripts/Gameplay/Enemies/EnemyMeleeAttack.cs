using PrimeTween;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float Delay;
    [SerializeField]
    private float Duration;
    [SerializeField]
    private Ease Ease;
    [SerializeField]
    private float EndXZScale;

    private void Start()
    {
        var startValue = new Vector3(0.0f, transform.localScale.y, 0.0f);
        transform.localScale = startValue;

        Tween.Scale(transform,
            endValue: new Vector3(EndXZScale, transform.localScale.y, EndXZScale),
            startDelay: Delay,
            duration: Duration,
            ease: Ease
        ).OnComplete(() => Destroy(gameObject));
    }
}