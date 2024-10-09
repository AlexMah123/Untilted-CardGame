using System;
using System.Collections.Generic;

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
