using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheFool", menuName = "Upgrades/UpgradeDefiniton/TheFool")]
    public class TheFool : UpgradeDefinitionSO
    {
        [SerializeField] private int healthIncrease = 2;

        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            playerCardStats.maxHealth += healthIncrease;

            return (playerCardStats, enemyCardStats);
        }
        
    }
}