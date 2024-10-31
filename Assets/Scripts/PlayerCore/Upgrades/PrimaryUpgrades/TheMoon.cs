using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheMoon", menuName = "Upgrades/UpgradeDefiniton/TheMoon")]
    public class TheMoon : UpgradeDefinitionSO
    {
        [SerializeField] private int attackIncrease = 1;
        
        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            if (currentTurnCount % 2 == 0)
            {
                playerCardStats.attack += attackIncrease;
            }
            
            return (playerCardStats, enemyCardStats);
        }
    }
}