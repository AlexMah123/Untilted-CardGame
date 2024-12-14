using System;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheHighPriestess", menuName = "Upgrades/UpgradeDefiniton/TheHighPriestess")]
    public class TheHighPriestess : UpgradeDefinitionSO
    {
        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            return (playerCardStats, enemyCardStats);
        }
        
        public override void ApplyActivatableEffect(Player attachedPlayer, Player enemyPlayer,
            IAbilityInputData inputData)
        {
            Debug.Log($"{enemyPlayer.name}'s next choice is {enemyPlayer.GetChoice()}");
        }
    }
}