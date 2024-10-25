using System;
using System.Collections.Generic;
using LevelCore.LevelManager;
using PlayerCore;
using PlayerCore.Upgrades.UpgradeFactory;

namespace GameCore.SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public List<LevelCompletionData> levelCompletionData;

        public PlayerStats upgradedPlayerStats;
        public HashSet<UpgradeType> playerUnlockedUpgrades;
        public HashSet<UpgradeType> playerEquippedUpgrades;

        public GameData()
        {
            levelCompletionData = new();

            upgradedPlayerStats = new();
            playerUnlockedUpgrades = new();
            playerEquippedUpgrades = new();
        }
    }
}