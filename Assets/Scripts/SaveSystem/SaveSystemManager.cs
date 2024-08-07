using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance { get; private set; }

    private GameData gameData;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        //#TODO: pass data to other scripts so they can update the current state
        //#TODO: save the data to a file using data handler
    }

    public void LoadGame()
    {
        //#TODO: Load any save data from a file using data handler

        if(gameData == null)
        {
            NewGame();
        }

        //#TODO: push all the loaded data to all other scripts
    }
}
