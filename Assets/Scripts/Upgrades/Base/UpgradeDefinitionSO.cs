using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradeDefinitionSO : ScriptableObject
{
    public Image upgradeImage;
    public string upgradeName;
    
    [Multiline]
    public string upgradeDefinition;

    public bool isActivatable;

    public virtual void ApplyPassiveEffect(IPlayer player)
    {

    }

    public virtual void ApplyActivatableEffect(IPlayer player)
    {

    }

    public virtual void OnWinRound(IPlayer player)
    {

    }

    public virtual void OnLoseRound(IPlayer player)
    {

    }

    public virtual void OnDrawRound(IPlayer player)
    {

    }
}
