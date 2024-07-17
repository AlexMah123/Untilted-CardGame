using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyComponent : MonoBehaviour
{
    [Header("Runtime Value")]
    public int energyAmount;

    //declaration of events
    public event Action<int> OnEnergyModifiedEvent;

    public void InitializeComponent(PlayerStatsSO referencedStats)
    {
        energyAmount = referencedStats.energy;
    }

    public void IncreaseEnergy(int value)
    {
        ModifyDamageAmount(value);
    }

    public void DecreaseEnergy(int value)
    {
        ModifyDamageAmount(-1 * value);
    }

    #region Internal Function
    private void ModifyDamageAmount(int value)
    {
        energyAmount += value;

        //#TODO: broadcast event to PlayerHUD/EnemyHUD
        OnEnergyModifiedEvent?.Invoke(energyAmount);
    }
    #endregion
}
