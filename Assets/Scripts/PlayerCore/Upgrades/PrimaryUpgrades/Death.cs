using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "Death", menuName = "Upgrades/UpgradeDefiniton/Death")]
    public class Death : UpgradeDefinitionSO
    {
        [SerializeField] private int attackIncrease = 1;
        [SerializeField] private int damageTakenIncrease = 1;

        public override (PlayerStats playerstats, PlayerStats enemyStats)
            ApplyStatUpgrade(PlayerStats playerCardStats, PlayerStats enemyCardStats, int currentTurnCount)
        {
            playerCardStats.attack += attackIncrease;
            playerCardStats.damageTaken += damageTakenIncrease;
            
            return (playerCardStats, enemyCardStats);
        }
    }
}