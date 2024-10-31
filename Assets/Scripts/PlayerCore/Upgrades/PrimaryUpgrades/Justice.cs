using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "Justice", menuName = "Upgrades/UpgradeDefiniton/Justice")]
    public class Justice : UpgradeDefinitionSO
    {
        private bool hasBeenApplied = false;

        public override void Initialize()
        {
            hasBeenApplied = false;
        }

        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            return (playerCardStats, enemyCardStats);
        }

        public override void ApplyPassiveEffect(Player attachedPlayer, Player enemyPlayer, int currentTurnCount)
        {
            if (hasBeenApplied) return;
            
            //total players health, rounded up, split health
            var attachedPlayerHealth = attachedPlayer.HealthComponent.currentHealth;
            var enemyHealth = enemyPlayer.HealthComponent.currentHealth;
            var splitHealth = Mathf.CeilToInt((attachedPlayerHealth + enemyHealth) / 2f);
                
            attachedPlayer.HealthComponent.SetHealth(splitHealth);
            enemyPlayer.HealthComponent.SetHealth(splitHealth);

            Debug.Log($"Justice activated: {attachedPlayerHealth} | {enemyHealth} | {splitHealth}");
            hasBeenApplied = true;
        }
    }
}