using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Runtime Value")]
    public int healthAmount;

    //declaration of events
    public event Action<int> OnHealthModified;
    public event Action OnHealthZero;

    public void InitializeComponent(PlayerStatsSO referencedStats)
    {
        healthAmount = referencedStats.health;
    }

    public void IncreaseHealth(int value)
    {
        ModifyHealthAmount(value);
    }

    public void DecreaseHealth(int value)
    {
        ModifyHealthAmount(-1 * value);
    }

    #region Internal Function
    private void ModifyHealthAmount(int value)
    {
        healthAmount += value;

        healthAmount = Math.Max(healthAmount, 0);

        OnHealthModified?.Invoke(healthAmount);

        if(healthAmount <= 0)
        {
            OnHealthZero?.Invoke();
        }

    }
    #endregion
}
