using System;
using GameCore.TurnSystem;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheSun", menuName = "Upgrades/UpgradeDefiniton/TheSun")]
    public class TheSun : UpgradeDefinitionSO
    {
        [SerializeField] private int attackIncrease = 1;
        
        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            //on odd turns, increase attack
            if (currentTurnCount % 2 != 0)
            {
                playerCardStats.attack += attackIncrease;
            }
            
            return (playerCardStats, enemyCardStats);
        }
    }
}