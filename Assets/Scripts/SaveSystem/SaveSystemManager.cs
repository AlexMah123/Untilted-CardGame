using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<ISavableData> savableDataObjects;
    private FileDataHandler dataHandler;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

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

        dataHandler = new(Application.persistentDataPath, fileName);
    }

    private void Start()
    {
        BootstrapSaveData();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void HandleSceneLoaded(Scene arg0, LoadSceneMode mode)
    {
        BootstrapSaveData();
    }

    private void BootstrapSaveData()
    {
        //query for all objects that implement ISavableData, and Load to them.
        savableDataObjects = QueryAllSavableObjects();
        LoadGame();
    }

    #region Save System
    private void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        //retrieve data to other objects that implement interface in scene
        foreach (ISavableData savableDataObj in savableDataObjects)
        {
            savableDataObj.SaveData(ref gameData);
        }

        //save the data to a file using data handler
        dataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        //Load any save data from a file using data handler
        gameData = dataHandler.Load();

        //defaulted to create new save file if null
        if(gameData == null)
        {
            NewGame();
        }

        //push all the loaded data to other objects that implement interface in scene
        foreach(ISavableData savableDataObj in savableDataObjects)
        {
            savableDataObj.LoadData(gameData);
        }
    }
    #endregion

    #region Internal Methods
    private List<ISavableData> QueryAllSavableObjects()
    {
        IEnumerable<ISavableData> savableDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavableData>();

        return new(savableDataObjects);
    }
    #endregion
}
