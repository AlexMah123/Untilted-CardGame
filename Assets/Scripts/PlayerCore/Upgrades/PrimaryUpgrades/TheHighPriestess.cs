using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheHighPriestess", menuName = "Upgrades/UpgradeDefiniton/TheHighPriestess")]
    public class TheHighPriestess : UpgradeDefinitionSO
    {

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
        }
    }
}
