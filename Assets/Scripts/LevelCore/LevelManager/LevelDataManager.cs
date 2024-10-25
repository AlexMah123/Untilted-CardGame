using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.SaveSystem;
using GameCore.SaveSystem.Data;
using LevelCore.LevelConfig.Base;
using UnityEngine;

namespace LevelCore.LevelManager
{
    [Serializable]
    public class LevelCompletionData
    {
        public string levelName;
        public bool isCompleted;
    }

    public class LevelDataManager : MonoBehaviour, ISavableData
    {
        public static LevelDataManager Instance;

        public LevelConfigSO currentSelectedLevelSO;

        [Header("Level Completion Data")] 
        public List<LevelCompletionData> totalLevels;

        public event Action OnSaveDataLoaded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public bool IsLevelCompleted(string levelName)
        {
            return totalLevels.Any(level => level.levelName == levelName && level.isCompleted);
        }


        #region SaveInterface

        public void LoadData(GameData data)
        {
            foreach (var savedLevel in data.levelCompletionData)
            {
                var levelData = totalLevels.Find(level => level.levelName == savedLevel.levelName);

                if (levelData != null)
                {
                    levelData.isCompleted = savedLevel.isCompleted;
                }
            }

            OnSaveDataLoaded?.Invoke();
        }

        public void SaveData(ref GameData data)
        {
        }

        #endregion
    }
}