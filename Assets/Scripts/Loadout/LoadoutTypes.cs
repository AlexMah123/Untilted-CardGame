using System;
using System.Collections.Generic;

[Serializable]
public struct LoadoutData
{
    public LoadoutData(List<UpgradeDefinitionSO> _totalUpgrades, HashSet<UpgradeDefinitionSO> _totalUnlockedUpgrades, HashSet<UpgradeDefinitionSO> _currentActiveUpgrades)
    {
        totalUpgradesInGame = _totalUpgrades;
        totalUnlockedUpgrades = _totalUnlockedUpgrades;
        totalEquippedUpgrades = _currentActiveUpgrades;
    }

    public List<UpgradeDefinitionSO> totalUpgradesInGame;
    public HashSet<UpgradeDefinitionSO> totalUnlockedUpgrades;
    public HashSet<UpgradeDefinitionSO> totalEquippedUpgrades;
}


public struct LoadoutCardGOInfo
{
    public LoadoutCardGOInfo(UpgradeDefinitionSO _upgradeSO)
    {
        upgradeSO = _upgradeSO;
    }

    public UpgradeDefinitionSO upgradeSO;
}