using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
