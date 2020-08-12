using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Statistics;

using UnityEngine;

namespace Persistence {
    public static class PersistenceManager
    {
        public static void SaveGame() {
            BinaryFormatter formatter = new BinaryFormatter();
            string filePath = Application.persistentDataPath + "/saveData.avtx";
            FileStream stream = new FileStream(filePath, FileMode.Create);
            SaveData saveData = new SaveData();
            formatter.Serialize(stream, saveData);
            stream.Close();
        }

        public static void LoadGame() {
            string filePath = Application.persistentDataPath + "/saveData.avtx";
            if (File.Exists(filePath)) {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(filePath, FileMode.Open);
                try {
                    SetLoadedData(formatter.Deserialize(stream) as SaveData);
                } catch (SerializationException) {
                    Debug.LogWarning("Could Not Serialize Saved Data");
                }
                stream.Close();
            } else {
                Debug.LogWarning("No Save File Found at Path: " + filePath);
            }
        }

        private static void SetLoadedData(SaveData saveData) {
            ScoreManager.sharedInstance.SetHighScores(saveData.highScores);
            CoinManager.sharedInstance.SetCoinsTotal(saveData.coinsCollected);
            AchievementManager.sharedInstance.SetAchievementStatusArray(saveData.achievementStatus);
            SetLoadedStatistics(saveData);
        }

        private static void SetLoadedStatistics(SaveData saveData) {
            StatsManager statsManager = StatsManager.sharedInstance;
            statsManager.SetRunsCompleted(saveData.totalRunsCompleted);
            statsManager.totalScoreAchieved  = saveData.totalScoreAchieved;
            statsManager.totalCoinsCollected = saveData.totalCoinsCollected;
            statsManager.totalModifiersUsed  = saveData.totalModifiersUsed;
            statsManager.totalFlipCount      = saveData.totalFlipCount;
            statsManager.totalDashCount      = saveData.totalDashCount;
            statsManager.totalDelayCount     = saveData.totalDelayCount;
            statsManager.totalNearMissCount  = saveData.totalNearMissCount;
            statsManager.totalTimeSurvived   = saveData.totalTimeSurvived;
            statsManager.RecalculateAverageStats();
        }
    }
}