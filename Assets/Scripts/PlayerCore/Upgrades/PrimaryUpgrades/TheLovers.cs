using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheLovers", menuName = "Upgrades/UpgradeDefiniton/TheLovers")]
    public class TheLovers : UpgradeDefinitionSO
    {
        [SerializeField] private int selfHeal = 1;
        [SerializeField] private int enemyHeal = 1;

        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            return (playerCardStats, enemyCardStats);
        }

        public override void OnDrawRound(Player attachedPlayer, Player enemyPlayer)
        {
            attachedPlayer.HealthComponent.IncreaseHealth(selfHeal);
            enemyPlayer.HealthComponent.IncreaseHealth(enemyHeal);
        }
    }
}