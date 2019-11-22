using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadManager
{
    public static void SaveGame() {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "saveData.avg";
        FileStream stream = new FileStream(filePath, FileMode.Create);
        SaveData saveData = new SaveData();
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static void LoadGame() {
        string filePath = Application.persistentDataPath + "saveData.avg";
        if (File.Exists(filePath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
            SetLoadedData(formatter.Deserialize(stream) as SaveData);
            stream.Close();
        } else {
            Debug.LogWarning("No Save File Found at Path: " + filePath);
        }
    }

    private static void SetLoadedData(SaveData saveData) {
        ScoreManager.sharedInstance.SetHighScore(saveData.highscore);
        CoinManager.sharedInstance.SetCoinsTotal(saveData.coins);
    }
}