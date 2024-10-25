using System.Collections.Generic;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.Collection
{
    [CreateAssetMenu(menuName = "Upgrades/Templates/TotalUpgradeSO")]
    public class UpgradeCollectionSO : ScriptableObject
    {
        public List<UpgradeDefinitionSO> upgradeList = new();
    }
}