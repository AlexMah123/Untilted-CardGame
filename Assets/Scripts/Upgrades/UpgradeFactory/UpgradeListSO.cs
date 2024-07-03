using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Templates/TotalUpgradeSO")]
public class UpgradeListSO : ScriptableObject
{
    public List<UpgradeDefinitionSO> upgradeList = new();
}
