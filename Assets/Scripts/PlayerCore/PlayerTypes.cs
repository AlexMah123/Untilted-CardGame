using System;
using System.Collections.Generic;
using PlayerCore.AIPlayer.AIModule.Base;
using Upgrades.Base;

namespace PlayerCore
{
    [Serializable]
    public struct FPlayerData
    {
        public PlayerStatsSO baseStatsConfig;
        public AIDecision aiModule;
        public PlayerType playerType;

        public List<UpgradeDefinitionSO> upgradesEquipped;
    }

    public enum PlayerType
    {
        Human,
        Computer,
    }
}