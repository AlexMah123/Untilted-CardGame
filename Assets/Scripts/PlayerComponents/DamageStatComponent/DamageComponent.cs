using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [Header("Runtime Value")]
    public int damageAmount;

    //declaration of events
    public event Action<int> OnDamageModifiedEvent;

    public void InitializeComponent(PlayerStatsSO referencedStats)
    {
        damageAmount = referencedStats.damage;
    }

    public void IncreaseDamage(int value)
    {
        ModifyDamageAmount(value);
    }

    public void DecreaseDamage(int value)
    {
        ModifyDamageAmount(-1 * value);
    }

    public void DealDamage(Player target, int amount)
    {
        target.HealthComponent.DecreaseHealth(amount);
    }

    #region Internal Function
    private void ModifyDamageAmount(int value)
    {
        damageAmount += value;

        //#TODO: broadcast event to total stats display
        OnDamageModifiedEvent?.Invoke(damageAmount);
    }
    #endregion
}