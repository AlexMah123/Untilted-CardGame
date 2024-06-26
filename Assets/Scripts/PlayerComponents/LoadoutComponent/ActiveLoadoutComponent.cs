using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLoadoutComponent : MonoBehaviour
{
    [Range(2, 10)]
    public int maxLimitOfLoadout;
    public List<UpgradeDefinitionSO> upgradeSlots = new();

    [HideInInspector]
    public IPlayer attachedPlayer;

    private void Start()
    {
        if (attachedPlayer == null)
        {
            throw new MissingReferenceException("ActiveLoadout does not have an reference to player");
        }
    }

    [ContextMenu("ActiveLoadout/Apply Passive Effect")]
    public void ApplyPassiveEffects()
    {
        foreach(var upgrade in upgradeSlots)
        {
            upgrade.ApplyPassiveEffect(attachedPlayer);
        }
    }

    [ContextMenu("ActiveLoadout/Apply OnWin Effect")]
    public void ApplyOnWinEffects()
    {
        foreach (var upgrade in upgradeSlots)
        {
            upgrade.OnWinRound(attachedPlayer);
        }
    }


    [ContextMenu("ActiveLoadout/Apply OnLose Effect")]
    public void ApplyOnLoseEffect()
    {
        foreach (var upgrade in upgradeSlots)
        {
            upgrade.OnWinRound(attachedPlayer);
        }
    }

    [ContextMenu("ActiveLoadout/Apply OnDraw Effect")]
    public void ApplyOnDrawEffect()
    {
        foreach (var upgrade in upgradeSlots)
        {
            upgrade.OnWinRound(attachedPlayer);
        }
    }

    public List<UpgradeDefinitionSO> FetchActiveLoadout()
    {
        return upgradeSlots;
    }

    public void AddToLoadout(UpgradeDefinitionSO upgrade)
    {
        upgradeSlots.Add(upgrade);
    }

    #region Debug
    [ContextMenu("Debug/Debug Attached Player")]
    public void DebugAttachedPlayer()
    {
        Debug.Log($"{attachedPlayer.GetType()}");
    }
    #endregion

}
