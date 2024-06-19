using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatComponent : MonoBehaviour
{
    [Header("Stats Component Config")]
    public PlayerStatsSO stats;

    //declaration of events
    public event Action OnHealthModifiedEvent;

    public void IncreaseHealth(int value)
    {
        ModifyHealth(value);
    }

    public void DecreaseHealth(int value)
    {
        ModifyHealth(-1 * value);
    }

    #region Internal Function
    private void ModifyHealth(int value)
    {
        stats.health += value;


        //#TODO: broadcast event to PlayerHUD/EnemyHUD
        OnHealthModifiedEvent?.Invoke();
    }
    #endregion
}
