using System;
using UnityEngine;

public class EnergyComponent : MonoBehaviour
{
    [Header("Runtime Value")]
    public int energyAmount;

    //declaration of events
    public event Action<int> OnEnergyModified;

    public void InitializeComponent(PlayerStats referencedStats)
    {
        energyAmount = referencedStats.energy;
        
        OnEnergyModified?.Invoke(energyAmount);
    }

    public void IncreaseEnergy(int value)
    {
        ModifyEnergyAmount(value);
    }

    public void DecreaseEnergy(int value)
    {
        ModifyEnergyAmount(-1 * value);
    }

    #region Internal Function
    private void ModifyEnergyAmount(int value)
    {
        energyAmount += value;

        //#TODO: broadcast event to PlayerHUD/EnemyHUD
        OnEnergyModified?.Invoke(energyAmount);
    }
    #endregion
}
