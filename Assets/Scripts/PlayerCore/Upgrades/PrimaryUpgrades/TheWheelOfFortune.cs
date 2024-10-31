using System;
using GameCore;
using PlayerCore.Upgrades.Base;
using UnityEngine;
using Random = UnityEngine.Random;


namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheWheelOfFortune", menuName = "Upgrades/UpgradeDefiniton/TheWheelOfFortune")]
    public class TheWheelOfFortune : UpgradeDefinitionSO
    {
        [SerializeField] private float winChance = 0.05f;
        [SerializeField] private float drawChance = 0.2f;

        public override GameResult ApplyResultAlteringEffect(GameResult initialResult, Player attachedPlayer, Player enemyPlayer)
        {
            if (initialResult == GameResult.Lose)
            {
                int chance = Random.Range(0, 101);
                
                float winThreshold = winChance * 100f;
                float drawThreshold = (winChance + drawChance) * 100f; 
                
                if(chance < winThreshold) return GameResult.Win;
                if(chance < drawThreshold) return GameResult.Draw;
                return GameResult.Lose;
            }
            
            //no change
            return initialResult;
        }

        public override (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount)
        {
            return (playerCardStats, enemyCardStats);
        }
    }
}