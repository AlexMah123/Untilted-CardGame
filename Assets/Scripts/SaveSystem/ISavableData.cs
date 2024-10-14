using System;
using SaveSystem.Data;

namespace SaveSystem
{
    public interface ISavableData
    {
        event Action OnSaveDataLoaded;

        void LoadData(GameData data);
        void SaveData(ref GameData data);
    }
}
