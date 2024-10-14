using System;
using System.IO;
using Newtonsoft.Json;
using SaveSystem.Data;
using UnityEngine;

namespace SaveSystem
{
    public class FileDataHandler
    {
        private string dataDirectoryPath = "";
        private string dataFileName = "";

        public FileDataHandler(string dataDirectoryPath, string dataFileName)
        {
            this.dataDirectoryPath = dataDirectoryPath;
            this.dataFileName = dataFileName;
        }

        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    //opens file and reads the json
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    //convert json to object
                    loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file:" + fullPath + "\n" + e);
                }
            }

            return loadedData;
        }

        public void Save(GameData data)
        {
            string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

            try
            {
                //create file if it does not exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                //converts data to json format
                string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);

                //create file and writes the data
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file:" + fullPath + "\n" + e);
            }
        }

        public void ClearData()
        {
            string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

            try
            {
                if(File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                else
                {
                    Debug.LogWarning("File does not exist");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to clear save data from file:" + fullPath + "\n" + e);
            }
        }

    }
}
