using GameCore;
using GameCore.SaveSystem.Data;
using LevelCore.LevelManager;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace PlayerCore.HumanPlayer
{
    public class HumanPlayer : Player
    {
        protected override void LoadComponents()
        {
            base.LoadComponents();

            ActiveLoadoutComponent.InitializeComponent(this, GameManager.Instance.aiPlayer, finalStats);
        }

        #region SaveSystem Override

        protected override void LoadPlayerData(GameData data)
        {
            //load from LevelDataManager
            var playerData = LevelDataManager.Instance.currentSelectedLevelSO.humanFPlayer;

            baseStatsConfig = playerData.baseStatsConfig;

            //load stats upgrade from save file
            progressionStats = new PlayerStats
            {
                maxHealth = data.upgradedPlayerStats.maxHealth,
                attack = data.upgradedPlayerStats.attack,
                cardSlots = data.upgradedPlayerStats.cardSlots,
                energy = data.upgradedPlayerStats.energy
            };

            //load cardUpgrades from save file
            foreach (UpgradeType upgradeType in data.playerEquippedUpgrades)
            {
                ActiveLoadoutComponent.AddUpgradeToLoadout(upgradeType);
            }
        }

        #endregion
    }
}