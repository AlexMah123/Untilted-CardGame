using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheDevil", menuName = "Upgrades/UpgradeDefiniton/TheDevil")]
    public class TheDevil : UpgradeDefinitionSO
    {
        [SerializeField] private int sealDuration = 1;
        
        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            return (playerCardStats, enemyCardStats);
        }

        public override void OnDrawRound(Player attachedPlayer, Player enemyPlayer)
        {
            SealEnemyHandStyle(enemyPlayer);
        }

        public override void OnLoseRound(Player attachedPlayer, Player enemyPlayer)
        {
            SealEnemyHandStyle(enemyPlayer);
        }
        
        
        #region Internal
        private void SealEnemyHandStyle(Player enemyPlayer)
        {
            enemyPlayer.ChoiceComponent.SealChoice(enemyPlayer.GetChoice(), sealDuration);
        }
        #endregion
    }
}