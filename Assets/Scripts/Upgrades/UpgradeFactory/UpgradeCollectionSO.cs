using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Templates/TotalUpgradeSO")]
public class UpgradeCollectionSO : ScriptableObject
{
    public List<UpgradeDefinitionSO> upgradeList = new();
}
