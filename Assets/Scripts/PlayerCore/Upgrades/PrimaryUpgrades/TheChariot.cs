using System;
using PlayerCore.PlayerComponents;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheChariot", menuName = "Upgrades/UpgradeDefiniton/TheChariot")]
    public class TheChariot : UpgradeDefinitionSO
    {
        [SerializeField] private int choiceSealDuration = 1;
        
        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            return (playerCardStats, enemyCardStats);
        }

        public override void ApplyActivatableEffect(Player attachedPlayer, Player enemyPlayer,
            IAbilityInputData inputData)
        {

            if (inputData is ChoiceSealInputData choiceSealInputData)
            {
                SealEnemyHandStyle(enemyPlayer, choiceSealInputData);
            }
            else
            {
                Debug.LogWarning($"Invalid input data {inputData.GetType()}  , expecting ChoiceSealInputData");
            }
        }
        
        #region Internal
        private void SealEnemyHandStyle(Player enemyPlayer, ChoiceSealInputData choiceSealInputData)
        {
            enemyPlayer.ChoiceComponent.SealChoice(choiceSealInputData.choiceToSeal, choiceSealDuration);
        }
        #endregion
    }
}