using System;
using System.Collections.Generic;
using UnityEngine.Serialization;


[Serializable]
public struct PlayerData
{
    public PlayerStatsSO baseStatsConfig;
    public AIDecision AiModule;
    public PlayerType playerType;

    public List<UpgradeDefinitionSO> upgradesEquipped;
}

public enum PlayerType
{
    Human,
    Computer,
}
