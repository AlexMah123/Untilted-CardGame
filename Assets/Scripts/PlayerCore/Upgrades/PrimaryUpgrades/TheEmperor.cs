using System;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheEmperor", menuName = "Upgrades/UpgradeDefiniton/TheEmperor")]
    public class TheEmperor : UpgradeDefinitionSO
    {
        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            return (playerCardStats, enemyCardStats);
        }
        
        public override void ApplyActivatableEffect(Player attachedPlayer, Player enemyPlayer,
            IAbilityInputData inputData)
        {
            //based on input, remove target upgrade
            
            if (inputData is TargetUpgradeInputData targetUpgradeInputData)
            {
                enemyPlayer.ActiveLoadoutComponent.RemoveUpgradeFromLoadout(targetUpgradeInputData.targetUpgrade);
            }
            else
            {
                Debug.LogWarning($"Invalid input data: {inputData.GetType()}  , expecting TargetUpgradeInputData");
            }
        }
    }
}