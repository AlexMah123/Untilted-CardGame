using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct PlayerData
{
    public PlayerStatsSO StatsConfig;
    public AIDecision AiModule;
    public PlayerType playerType;
}

public enum PlayerType
{
    Human,
    Computer,
}
