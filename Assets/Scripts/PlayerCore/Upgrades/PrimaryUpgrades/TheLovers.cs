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
            PlayerStats enemyCardStats)
        {
            return (playerCardStats, enemyCardStats);
        }

        public override void ApplyPassiveEffect(Player attachedPlayer, Player enemyPlayer)
        {
            base.ApplyPassiveEffect(attachedPlayer, enemyPlayer);
        }

        public override void ApplyActivatableEffect(Player attachedPlayer, Player enemyPlayer)
        {
            base.ApplyActivatableEffect(attachedPlayer, enemyPlayer);
        }

        public override void OnWinRound(Player attachedPlayer, Player enemyPlayer)
        {
            base.OnWinRound(attachedPlayer, enemyPlayer);
        }

        public override void OnLoseRound(Player attachedPlayer, Player enemyPlayer)
        {
            base.OnLoseRound(attachedPlayer, enemyPlayer);
        }

        public override void OnDrawRound(Player attachedPlayer, Player enemyPlayer)
        {
            base.OnDrawRound(attachedPlayer, enemyPlayer);
            
            attachedPlayer.HealthComponent.IncreaseHealth(selfHeal);
            enemyPlayer.HealthComponent.IncreaseHealth(enemyHeal);
        }
    }
}
