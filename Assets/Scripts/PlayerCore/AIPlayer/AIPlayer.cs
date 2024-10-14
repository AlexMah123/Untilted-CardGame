using LevelManager;
using PlayerComponents;
using PlayerCore.AIPlayer.AIModule.Base;
using SaveSystem.Data;
using UnityEngine;
using Upgrades.Base;

namespace PlayerCore.AIPlayer
{
    public class AIPlayer : Player
    {
        public AIDecision aiModuleConfig;
    
        public override void LoadComponents()
        {
            base.LoadComponents();

            aiModuleConfig.InitializeAIConfig(ChoiceComponent);
        }

        public override GameChoice GetChoice()
        {
            if (aiModuleConfig == null)
            {
                throw new MissingComponentException("AI Module is not assigned");
            }

            return aiModuleConfig.MakeDecision();
        }

        protected override void LoadPlayerData(GameData data)
        {
            var computerPlayerData = LevelDataManager.Instance.currentSelectedLevelSO.aiFPlayer;

            baseStatsConfig = computerPlayerData.baseStatsConfig;
            aiModuleConfig = computerPlayerData.aiModule;

            //load data from levelconfig
            foreach (UpgradeDefinitionSO upgradeSO in computerPlayerData.upgradesEquipped)
            {
                ActiveLoadoutComponent.AddUpgradeToLoadout(upgradeSO);
            }
        }

    }
}
