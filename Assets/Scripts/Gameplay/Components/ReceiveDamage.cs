using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    [Header("Reference")]
    public Health Health;
    public HurtBox HurtBox;

    private void OnEnable()
    {
        HurtBox.OnHurt.AddListener(OnHurtBoxHurt);
    }

    private void OnDisable()
    {
        HurtBox.OnHurt.RemoveListener(OnHurtBoxHurt);
    }

    private void OnHurtBoxHurt(HitBox hitBox)
    {
        Health.CurrentHealth -= hitBox.Damage;
    }
}