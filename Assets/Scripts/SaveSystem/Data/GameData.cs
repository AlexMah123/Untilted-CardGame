using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int number;

    public Dictionary<string, bool> upgradesUnlocked;

    public GameData()
    {
        number = 0;
        upgradesUnlocked = new();
    }
}
