using UnityEngine;

public interface IDataPersistence
{
    // Reads data
    void LoadData(GameData data);

    // Modifies data
    void SaveData(ref GameData data);
}
