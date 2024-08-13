using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLoadoutComponent : MonoBehaviour
{
    [Header("Runtime Value")]
    public int maxLimitOfLoadout;
    public List<UpgradeDefinitionSO> upgradeSlots = new();

    [HideInInspector]
    public Player attachedPlayer;

    private void Start()
    {
        if (attachedPlayer == null)
        {
            throw new MissingReferenceException("ActiveLoadout does not have an reference to player");
        }
    }

    public void InitializeComponent(Player refPlayer, PlayerStatsSO referencedStats)
    {
        attachedPlayer = refPlayer;
        maxLimitOfLoadout = referencedStats.cardSlots;
    }

    [ContextMenu("ActiveLoadout/Apply Passive Effect")]
    public void ApplyPassiveEffects()
    {
        foreach(UpgradeDefinitionSO upgrade in upgradeSlots)
        {
            upgrade.ApplyPassiveEffect(attachedPlayer);
        }
    }

    [ContextMenu("ActiveLoadout/Apply OnWin Effect")]
    public void ApplyOnWinEffects()
    {
        foreach (UpgradeDefinitionSO upgrade in upgradeSlots)
        {
            upgrade.OnWinRound(attachedPlayer);
        }
    }


    [ContextMenu("ActiveLoadout/Apply OnLose Effect")]
    public void ApplyOnLoseEffect()
    {
        foreach (UpgradeDefinitionSO upgrade in upgradeSlots)
        {
            upgrade.OnWinRound(attachedPlayer);
        }
    }

    [ContextMenu("ActiveLoadout/Apply OnDraw Effect")]
    public void ApplyOnDrawEffect()
    {
        foreach (UpgradeDefinitionSO upgrade in upgradeSlots)
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
