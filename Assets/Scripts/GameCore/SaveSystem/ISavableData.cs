using System;
using GameCore.SaveSystem.Data;

namespace GameCore.SaveSystem
{
    public interface ISavableData
    {
        event Action OnSaveDataLoaded;

        void LoadData(GameData data);
        void SaveData(ref GameData data);
    }
}