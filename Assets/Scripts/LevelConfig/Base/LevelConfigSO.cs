using System.Collections.Generic;
using PlayerCore;
using UnityEngine;
using Upgrades.Base;

namespace LevelConfig.Base
{
    [CreateAssetMenu(menuName = "Level/LevelConfigSO")]
    public class LevelConfigSO : ScriptableObject
    {
        [Header("Human Player")]
        public PlayerData humanPlayer;

        [Header("Ai Player")]
        public PlayerData aiPlayer;

        [Header("UI Data")]
        public string levelName;
        public Sprite levelImage;

        [Header("Reward Data")]
        public List<UpgradeDefinitionSO> rewardList;

        public PlayerStats rewardStats;
    }
}
