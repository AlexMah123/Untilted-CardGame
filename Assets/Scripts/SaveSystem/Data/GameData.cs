using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public HashSet<UpgradeType> playerUnlockedUpgrades;
    public HashSet<UpgradeType> playerEquippedUpgrades;

    public GameData()
    {
        playerUnlockedUpgrades = new();
        playerEquippedUpgrades = new();
    }
}
