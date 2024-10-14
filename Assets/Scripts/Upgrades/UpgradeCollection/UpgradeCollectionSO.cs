using System.Collections.Generic;
using UnityEngine;
using Upgrades.Base;

namespace Upgrades.UpgradeCollection
{
    [CreateAssetMenu(menuName = "Upgrades/Templates/TotalUpgradeSO")]
    public class UpgradeCollectionSO : ScriptableObject
    {
        public List<UpgradeDefinitionSO> upgradeList = new();
    }
}
