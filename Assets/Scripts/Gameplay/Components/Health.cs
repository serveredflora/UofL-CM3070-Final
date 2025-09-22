using TriInspector;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    [Min(1)]
    public int MaxHealth = 1;

    [HideInInspector]
    public int CurrentHealth
    {
        get => _CurrentHealth;
        set
        {
            if (_CurrentHealth == value) return;

            int previous = _CurrentHealth;
            _CurrentHealth = value;
            OnChange?.Invoke();
            OnChangeInfo?.Invoke(previous, _CurrentHealth);

            if (_CurrentHealth <= 0) OnDeplete?.Invoke();
        }
    }

    [SerializeField]
    [ReadOnly]
    [ShowIf(nameof(IsPlaying))]
    private int _CurrentHealth;

    private bool IsPlaying => Application.isPlaying;

    public UnityEvent OnChange;
    public UnityEvent<int, int> OnChangeInfo;
    public UnityEvent OnDeplete;

    private void Awake()
    {
        _CurrentHealth = MaxHealth;
    }
}