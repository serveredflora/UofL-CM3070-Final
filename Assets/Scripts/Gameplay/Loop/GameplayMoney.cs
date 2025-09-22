using System;
using UnityEngine;
using UnityEngine.Events;

public class GameplayMoney : MonoBehaviour
{
    public UnityEvent OnChange;

    public Money TotalMoney => Money?.TotalMoney ?? default(Money);

    private PlayerInventoryMoney Money;

    public void Initialize(PlayerInventoryMoney money)
    {
        if (Money != null)
        {
            Money.OnChange -= MoneyChanged;
        }

        Money = money;
        Money.OnChange += MoneyChanged;
    }

    public void Cleanup()
    {
        if (Money == null)
        {
            return;
        }

        Money.OnChange -= MoneyChanged;
        Money = null;
    }

    private void MoneyChanged()
    {
        OnChange?.Invoke();
    }
}