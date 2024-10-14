using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

using SaveSystem.Data;

namespace SaveSystem
{
    public class SaveSystemManager : MonoBehaviour
    {
        public static SaveSystemManager Instance { get; private set; }

        [Header("File Storage Config")]
        [SerializeField] private string fileName;

        private GameData gameData;
        private FileDataHandler dataHandler;

        private List<ISavableData> savableDataObjectsInScene;
        private int objectsToLoadCount;
        private int objectsLoadedCount;

        public event Action OnAllSaveDataLoaded;

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
            if (Instance != null && Instance != this)
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

        private void HandleSceneLoaded(Scene arg0, LoadSceneMode mode)
        {
            BootstrapSaveData();
        }

        #region Save System
        private void NewGame()
        {
            gameData = new GameData();
        }

        public void SaveGame()
        {
            QueryAllSavableObjects();

            //retrieve data to other objects that implement interface in scene
            foreach (ISavableData savableDataObj in savableDataObjectsInScene)
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
            if (gameData == null)
            {
                NewGame();
            }

            //push all the loaded data to other objects that implement interface in scene
            foreach (ISavableData savableDataObj in savableDataObjectsInScene)
            {
                savableDataObj.OnSaveDataLoaded += HandleDataLoaded;
                savableDataObj.LoadData(gameData);
            }
        }
    
        [ContextMenu("SaveSysten/ClearSaveData")]
        public void ClearSaveData()
        {
            dataHandler.ClearData();
        }
    
        #endregion

        #region Internal Methods
        private void BootstrapSaveData()
        {
            //query for all objects that implement ISavableData, and Load to them.
            QueryAllSavableObjects();

            // Reset counters
            objectsToLoadCount = savableDataObjectsInScene.Count;
            objectsLoadedCount = 0;

            LoadGame();
        }

        private void HandleDataLoaded()
        {
            objectsLoadedCount++;

            if (objectsLoadedCount >= objectsToLoadCount)
            {
                //#DEBUG
                //Debug.Log("Finished loading all data");

                foreach (ISavableData savableDataObj in savableDataObjectsInScene)
                {
                    savableDataObj.OnSaveDataLoaded -= HandleDataLoaded;
                }

                Invoke(nameof(OnAllDataLoaded), 0f);
            }
        }

        private void OnAllDataLoaded()
        {
            OnAllSaveDataLoaded?.Invoke();
        }

        private void QueryAllSavableObjects()
        {
            IEnumerable<ISavableData> allSavableDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavableData>();

            savableDataObjectsInScene = new(allSavableDataObjects);
        }
        #endregion
    }
}
