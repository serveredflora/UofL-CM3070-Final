using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class HitBox : MonoBehaviour
{
    [Header("Settings")]
    [Min(1)]
    public int Damage = 1;

    public UnityEvent<HurtBox> OnHurtBoxEnter;

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<HurtBox>(out HurtBox hurtBox))
        {
            OnHurtBoxEnter?.Invoke(hurtBox);
            hurtBox.OnHurt?.Invoke(this);
        }
    }
}