using System;
using System.Collections.Generic;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace GameCore.LoadoutSelection
{
    [Serializable]
    public struct FLoadoutData
    {
        public FLoadoutData(List<UpgradeDefinitionSO> _totalUpgrades,
            HashSet<UpgradeDefinitionSO> _totalUnlockedUpgrades, HashSet<UpgradeDefinitionSO> _currentActiveUpgrades)
        {
            totalUpgradesInGame = _totalUpgrades;
            totalUnlockedUpgrades = _totalUnlockedUpgrades;
            totalEquippedUpgrades = _currentActiveUpgrades;
        }

        public List<UpgradeDefinitionSO> totalUpgradesInGame;
        public HashSet<UpgradeDefinitionSO> totalUnlockedUpgrades;
        public HashSet<UpgradeDefinitionSO> totalEquippedUpgrades;
    }


    [Serializable]
    public struct FLoadoutCardObj
    {
        public FLoadoutCardObj(UpgradeDefinitionSO _upgradeSO)
        {
            upgradeSO = _upgradeSO;
        }

        public UpgradeDefinitionSO upgradeSO;
    }

    public struct FLoadoutCardCreation
    {
        public FLoadoutCardCreation(Transform spawnParent)
        {
            parent = spawnParent;
        }

        public Transform parent;
    }
}