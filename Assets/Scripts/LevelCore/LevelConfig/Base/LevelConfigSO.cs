using System.Collections.Generic;
using PlayerCore;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace LevelCore.LevelConfig.Base
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