using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public static LevelDataManager Instance;

    public LevelConfigSO currentSelectedLevelSO;

    [Header("Level Completion Data")]
    public List<LevelCompletionData> totalLevels;

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

}

[Serializable]
public class LevelCompletionData
{
    public LevelConfigSO levelConfig;
    public bool isCompleted;
}
