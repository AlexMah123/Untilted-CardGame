using GameCore;
using GameCore.SaveSystem.Data;
using LevelCore.LevelManager;
using PlayerCore.AIPlayer.AIModule.Base;
using PlayerCore.PlayerComponents;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.AIPlayer
{
    public class AIPlayer : Player
    {
        public AIDecision aiModuleConfig;

        protected override void LoadComponents()
        {
            base.LoadComponents();

            ActiveLoadoutComponent.InitializeComponent(this, GameManager.Instance.player, finalStats);
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

        #region SaveSystemOverride

        protected override void LoadPlayerData(GameData data)
        {
            var aiPlayerData = LevelDataManager.Instance.currentSelectedLevelSO.aiFPlayer;

            baseStatsConfig = aiPlayerData.baseStatsConfig;
            aiModuleConfig = aiPlayerData.aiModule;

            //load data from levelconfig
            foreach (UpgradeDefinitionSO upgradeSO in aiPlayerData.upgradesEquipped)
            {
                ActiveLoadoutComponent.AddUpgradeToLoadout(upgradeSO);
            }
        }

        #endregion
    }
}