using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public List<LevelCompletionData> levelCompletionData;

    public HashSet<UpgradeType> playerUnlockedUpgrades;
    public HashSet<UpgradeType> playerEquippedUpgrades;

    public GameData()
    {
        levelCompletionData = new();

        playerUnlockedUpgrades = new();
        playerEquippedUpgrades = new();
    }
}
