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
    
        public override void LoadComponents()
        {
            base.LoadComponents();

            ActiveLoadoutComponent.InitializeComponent(this, GameManager.Instance.humanPlayer, currentStats);
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
            var computerPlayerData = LevelDataManager.Instance.currentSelectedLevelSO.aiFPlayer;

            baseStatsConfig = computerPlayerData.baseStatsConfig;
            aiModuleConfig = computerPlayerData.aiModule;

            //load data from levelconfig
            foreach (UpgradeDefinitionSO upgradeSO in computerPlayerData.upgradesEquipped)
            {
                ActiveLoadoutComponent.AddUpgradeToLoadout(upgradeSO);
            }
        }
        #endregion

    }
}
