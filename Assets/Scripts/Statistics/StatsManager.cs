using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    //Reference Variables
    public static StatsManager sharedInstance;

    //Current Run Statistics
    private int modifiersUsed;
    private int flipCount;
    private int dashCount;
    private int delayCount;
    private int nearMissCount;
    private float timeSurvived;

    //Cumulative Statistics
    private int runsCompleted = 0;
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
    public float averageModifiers;
    public float averageFlipCount;
    public float averageDashCount;
    public float averageDelayCount;
    public float averageNearMissCount;
    public float averageTimeSurvived;

    //State Variables
    private bool trackRunTime = false;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void OnSceneChange() {
        if (SceneManager.GetActiveScene().name == "PlayScene") {
            ToggleRunTimeTracking(true);
            ResetStats();
        }
        if (SceneManager.GetActiveScene().name == "GameOver") {
            ToggleRunTimeTracking(false);
            DisplayRunStats();
            UpdateTotalStats();
            UpdateAverageStats(); 
            PersistenceManager.SaveGame();
        }
        if (SceneManager.GetActiveScene().name == "Statistics") {
            DisplayAllStats();
        }
    }   //Called Through Singleton

    private void ResetStats() {
        modifiersUsed = 0;
        flipCount = 0;
        dashCount = 0;
        delayCount = 0;
        nearMissCount = 0;
        timeSurvived = 0f;
    }

    private void UpdateTotalStats() {
        runsCompleted++;
        totalScoreAchieved  += ScoreManager.sharedInstance.GetCurrentScore();
        totalCoinsCollected += CoinManager.sharedInstance.GetCoinsCollected();
        totalModifiersUsed  += modifiersUsed;
        totalFlipCount      += flipCount;
        totalDashCount      += dashCount;
        totalDelayCount     += delayCount;
        totalNearMissCount  += nearMissCount;
        totalTimeSurvived   += timeSurvived;
    }

    private void UpdateAverageStats() {
        averageScore         = (float) totalScoreAchieved  / runsCompleted;
        averageCoins         = (float) totalCoinsCollected / runsCompleted;
        averageModifiers     = (float) totalModifiersUsed  / runsCompleted;
        averageFlipCount     = (float) totalFlipCount      / runsCompleted;
        averageDashCount     = (float) totalDashCount      / runsCompleted;
        averageDelayCount    = (float) totalDelayCount     / runsCompleted;
        averageNearMissCount = (float) totalNearMissCount  / runsCompleted;
        averageTimeSurvived  = (float) totalTimeSurvived   / runsCompleted;
    }

    private void DisplayRunStats() {
        FindStatObjectByTag("ScoreStat").text    = ScoreManager.sharedInstance.GetCurrentScore().ToString();
        FindStatObjectByTag("CoinStat").text     = CoinManager.sharedInstance.GetCoinsCollected().ToString();
        FindStatObjectByTag("ModifierStat").text = modifiersUsed.ToString();
        FindStatObjectByTag("FlipStat").text     = flipCount.ToString();
        FindStatObjectByTag("DashStat").text     = dashCount.ToString();
        FindStatObjectByTag("DelayStat").text    = delayCount.ToString();
        FindStatObjectByTag("NearMissStat").text = nearMissCount.ToString();
        FindStatObjectByTag("TimeStat").text     = TimeToString(timeSurvived);
    }

    private void DisplayAllStats() {
        DisplayHighScores();
        DisplayLifetimeStats();
        DisplayAverageStats();
    }

    #region Helper Method For DisplayStats
    private void DisplayHighScores() {
        int[] highScores = ScoreManager.sharedInstance.GetHighScores();
        for(int i = 0; i < highScores.Length; i++) {
            if (highScores[i] == 0) {
                FindStatObjectByName("Score " + (i + 1) + " Number").text = "N/A";
            } else {
                FindStatObjectByName("Score " + (i + 1) + " Number").text = highScores[i].ToString();
            }
        }
    }

    private void DisplayLifetimeStats() {
        FindStatObjectByName("T Runs Number").text       = runsCompleted.ToString();
        FindStatObjectByName("T Coins Number").text      = totalCoinsCollected.ToString();
        FindStatObjectByName("T Modifiers Number").text  = totalModifiersUsed.ToString();
        FindStatObjectByName("T Flips Number").text      = totalFlipCount.ToString();
        FindStatObjectByName("T Dashes Number").text     = totalDashCount.ToString();
        FindStatObjectByName("T Delays Number").text     = totalDelayCount.ToString();
        FindStatObjectByName("T NearMisses Number").text = totalNearMissCount.ToString();
        FindStatObjectByName("T Time Number").text       = TimeToString(totalTimeSurvived);
    }

    private void DisplayAverageStats() {
        FindStatObjectByName("A Score Number").text      = AverageToString(averageScore);
        FindStatObjectByName("A Coins Number").text      = AverageToString(averageCoins);
        FindStatObjectByName("A Modifiers Number").text  = AverageToString(averageModifiers);
        FindStatObjectByName("A Flips Number").text      = AverageToString(averageFlipCount);
        FindStatObjectByName("A Dashes Number").text     = AverageToString(averageDashCount);
        FindStatObjectByName("A Delays Number").text     = AverageToString(averageDelayCount);
        FindStatObjectByName("A NearMisses Number").text = AverageToString(averageNearMissCount);
        FindStatObjectByName("A Time Number").text       = TimeToString(averageTimeSurvived);
    }

    private TextMeshProUGUI FindStatObjectByTag(string tag) {
        return GameObject.FindGameObjectWithTag(tag).GetComponent<TextMeshProUGUI>();
    }

    private TextMeshProUGUI FindStatObjectByName(string name) {
        foreach (TextMeshProUGUI statObject in Resources.FindObjectsOfTypeAll<TextMeshProUGUI>()) {
            if (statObject.gameObject.name == name) {
                return statObject.gameObject.GetComponent<TextMeshProUGUI>();
            }
        }
        return null;
    }

    private string AverageToString(float average) {
        if (average < 10) {
            return average.ToString("F2");
        } else if (average < 100) {
            return average.ToString("F1");
        } else {
            return average.ToString("F0");
        }
    }

    private string TimeToString(float seconds) {
        if (seconds < 60) {
            return seconds.ToString("F1") + "s";
        }
        int minutes = (int) (seconds / 60);
        seconds = seconds - (minutes * 60);
        if (minutes < 60) {
            return minutes.ToString() + "m " + seconds.ToString("F0") + "s";
        }
        int hours = minutes / 60;
        minutes = minutes - (hours * 60);
        if (hours < 24) {
            return hours.ToString() + "h " + minutes.ToString() + "m";
        }
        int days = hours / 24;
        hours = hours - (days * 24);
        return days.ToString() + "d " + hours.ToString() + "h ";
    }
    #endregion

    private void Update() {
        TickRunTime();
    }

    private void TickRunTime() {
        if (trackRunTime) {
            timeSurvived += Time.deltaTime;
        }
    }

    private void ToggleRunTimeTracking(bool track) {
        trackRunTime = track;
    }

    //Public Methods
    public void AddModifier() {
        modifiersUsed++;
    }

    public void AddFlip() {
        flipCount++;
    }

    public void AddDash() {
        dashCount++;
    }

    public void AddDelay() {
        delayCount++;
    }

    public void AddNearMiss() {
        nearMissCount++;
    }

    public void RecalculateAverageStats() {
        UpdateAverageStats();
    }

    //Get & Set Methods
    public void SetRunsCompleted(int totalRunsCompleted) {
        this.runsCompleted = totalRunsCompleted;
    }

    public int GetRunsCompleted() {
        return runsCompleted;
    }
}
