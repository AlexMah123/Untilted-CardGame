using System;
using System.Collections.Generic;


[Serializable]
public struct PlayerData
{
    public PlayerStatsSO StatsConfig;
    public AIDecision AiModule;
    public PlayerType playerType;

    public List<UpgradeDefinitionSO> upgradesEquipped;
}

public enum PlayerType
{
    Human,
    Computer,
}
