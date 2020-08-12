using System.Diagnostics.CodeAnalysis;

using Statistics;

namespace Persistence {
    
    [System.Serializable]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class SaveData
    {
        //Essential Data
        public int[] highScores;
        public int coinsCollected;
        public int totalRunsCompleted;

        //Cumulative Statistics
        public int totalScoreAchieved;
        public int totalCoinsCollected;
        public int totalModifiersUsed;
        public int totalFlipCount;
        public int totalDashCount;
        public int totalDelayCount;
        public int totalNearMissCount;
        public float totalTimeSurvived;

        //Average Statistics Per Run
        public float averageScore;
        public float averageCoins;
        public float averageModifiersUsed;
        public float averageFlipCount;
        public float averageDashCount;
        public float averageDelayCount;
        public float averageNearMissCount;
        public float averageTimeSurvived;

        //Achievement Status
        public bool[] achievementStatus;

        public SaveData() {
            GetEssentialData();
            GetCumulativeStatistics();
            GetAverageStatistics();
            GetAchievementsStatus();
        }

        private void GetEssentialData() {
            highScores = ScoreManager.sharedInstance.GetHighScores();
            coinsCollected = CoinManager.sharedInstance.GetCoinsTotal();
            totalRunsCompleted = StatsManager.sharedInstance.GetRunsCompleted();
        }

        private void GetCumulativeStatistics() {
            StatsManager statsManager = StatsManager.sharedInstance;
            totalScoreAchieved  = statsManager.totalScoreAchieved;
            totalCoinsCollected = statsManager.totalCoinsCollected;
            totalModifiersUsed  = statsManager.totalModifiersUsed;
            totalFlipCount      = statsManager.totalFlipCount;
            totalDashCount      = statsManager.totalDashCount;
            totalDelayCount     = statsManager.totalDelayCount;
            totalNearMissCount  = statsManager.totalNearMissCount;
            totalTimeSurvived   = statsManager.totalTimeSurvived;
        }

        private void GetAverageStatistics() {
            StatsManager statsManager = StatsManager.sharedInstance;
            averageScore         = statsManager.averageScore;
            averageCoins         = statsManager.averageCoins;
            averageModifiersUsed = statsManager.averageModifiers;
            averageFlipCount     = statsManager.averageFlipCount;
            averageDashCount     = statsManager.averageDashCount;
            averageDelayCount    = statsManager.averageDelayCount;
            averageNearMissCount = statsManager.averageNearMissCount;
            averageTimeSurvived  = statsManager.averageTimeSurvived;
        }

        private void GetAchievementsStatus() {
            achievementStatus = AchievementManager.sharedInstance.GetAchievementStatusArray();
        }
    }
}