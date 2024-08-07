using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LoadoutData
{
    public LoadoutData(List<UpgradeDefinitionSO> _totalUpgrades, List<UpgradeDefinitionSO> _totalUnlockedUpgrades, List<UpgradeDefinitionSO> _currentActiveUpgrades)
    {
        totalUpgrades = _totalUpgrades;
        totalUnlockedUpgrades = _totalUnlockedUpgrades;
        currentActiveUpgrades = _currentActiveUpgrades;
    }

    public List<UpgradeDefinitionSO> totalUpgrades;
    public List<UpgradeDefinitionSO> totalUnlockedUpgrades;
    public List<UpgradeDefinitionSO> currentActiveUpgrades;
}


public struct LoadoutCardGOInfo
{
    public LoadoutCardGOInfo(UpgradeDefinitionSO _upgradeSO)
    {
        upgradeSO = _upgradeSO;
    }

    public UpgradeDefinitionSO upgradeSO;
}