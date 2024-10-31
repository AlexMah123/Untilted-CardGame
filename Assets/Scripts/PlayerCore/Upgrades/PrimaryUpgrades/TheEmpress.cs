using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheEmpress", menuName = "Upgrades/UpgradeDefiniton/TheEmpress")]
    public class TheEmpress : UpgradeDefinitionSO
    {
        [SerializeField] private int healAmount = 1;
        [SerializeField] private int repeatInterval = 3;
        
        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            return (playerCardStats, enemyCardStats);
        }

        public override void ApplyPassiveEffect(Player attachedPlayer, Player enemyPlayer, int currentTurnCount)
        {
            if (currentTurnCount % repeatInterval == 0)
            {
                attachedPlayer.HealthComponent.IncreaseHealth(healAmount);
            }
        }
    }
}