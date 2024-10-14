using System.Collections.Generic;
using UnityEngine;

using PlayerCore;
using Upgrades.Base;

namespace LevelConfig.Base
{
    [CreateAssetMenu(menuName = "Level/LevelConfigSO")]
    public class LevelConfigSO : ScriptableObject
    {
        [Header("Human Player")]
        public FPlayerData humanFPlayer;

        [Header("Ai Player")]
        public FPlayerData aiFPlayer;

        [Header("UI Data")]
        public string levelName;
        public Sprite levelImage;

        [Header("Reward Data")]
        public List<UpgradeDefinitionSO> rewardList;

        public PlayerStats rewardStats;
    }
}
