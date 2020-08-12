using System.Diagnostics.CodeAnalysis;
using Persistence;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statistics {
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager sharedInstance;

        //Configuration Parameters
        [SerializeField] Achievement[] achievements = null;
        [SerializeField] AchievementInterface achievementPrefab = null;

        //State Variables
        private Transform achievementParent = null;
        private bool[] statusArray;

        //Internal Methods
        private void Awake() {
            SetSharedInstance();
        }

        private void SetSharedInstance() {
            sharedInstance = this;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnSceneChange() {
            if (SceneManager.GetActiveScene().name == "GameOver") {
                CheckForStatusArray();
                CheckAchievementStatus();
                PersistenceManager.SaveGame();
            }
            if (SceneManager.GetActiveScene().name == "Statistics") {
                CheckForStatusArray();
                DisplayAchievements();
            }
        }   //Called Through Singleton

        private void CheckForStatusArray() {
            if (statusArray == null || statusArray.Length == 0) {
                statusArray = new bool[achievements.Length];
                for(int index = 0; index < statusArray.Length; index++) {
                    statusArray[index] = false;
                }
            }
        }

        private void CheckAchievementStatus() {
            StatsManager statManager = StatsManager.sharedInstance;
            for(int index = 0; index < achievements.Length; index++) {
                if (statusArray[index]) {
                    continue;
                }
                Achievement currentAchievement = achievements[index];
                if (statManager.GetStat(currentAchievement.goalStatistic) >= currentAchievement.goalThreshold) {
                    if (currentAchievement.conditionStatistic == StatsManager.Stat.Null) {
                        statusArray[index] = true;
                        AwardCoinReward(currentAchievement);
                        continue;
                    }
                    if (statManager.GetStat(currentAchievement.conditionStatistic) <= currentAchievement.conditionThreshold) {
                        statusArray[index] = true;
                        AwardCoinReward(currentAchievement);
                    }
                }
            }
        }

        private void AwardCoinReward(Achievement achievement) {
            CoinManager.sharedInstance.AddCoins(achievement.coinReward);
        }

        private void DisplayAchievements() {
            FindAchievementParent();
            for (int index = 0; index < achievements.Length; index++) {
                Achievement currentAchievement = achievements[index];
                AchievementInterface newAchievement = Instantiate(achievementPrefab, achievementParent, true);
                newAchievement.SetInterfaceObjectParameters(currentAchievement, statusArray[index]);
            }
        }

        private void FindAchievementParent() {
            foreach (RectTransform canvasObject in Resources.FindObjectsOfTypeAll<RectTransform>()) {
                if (canvasObject.gameObject.name == "AchievementListContent") {
                    achievementParent = canvasObject;
                }
            }
            if (!achievementParent) {
                Debug.LogError("No Parent Object found for Achievements to be placed under.");
            }
        }

        //Public Methods
        public bool[] GetAchievementStatusArray() {
            return statusArray;
        }

        public void SetAchievementStatusArray(bool[] loadedStatusArray) {
            statusArray = loadedStatusArray;
            CheckForStatusArray();
        }
    }
}
