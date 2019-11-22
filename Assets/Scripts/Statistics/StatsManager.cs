using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsManager : MonoBehaviour
{
    //Reference Variables
    public static StatsManager sharedInstance;

    //Configuration Parameters

    //State Variales
    private int powerupCount;
    private int flipCount;
    private int dashCount;
    private int delayCount;
    private int nearMissCount;
    private int timeSurvived;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void OnSceneChange() {
        if (SceneManager.GetActiveScene().name == "PlayScene") {
            ResetStats();
        }
        if (SceneManager.GetActiveScene().name == "GameOver") {
            DisplayStats();
        }
    }   //Called Through Singleton

    private void ResetStats() {
        powerupCount = 0;
        flipCount = 0;
        dashCount = 0;
        delayCount = 0;
        nearMissCount = 0;
        timeSurvived = 0;
    }

    private void DisplayStats() {
        FindStatObject("ScoreStat").text = ScoreManager.sharedInstance.GetCurrentScore().ToString();
        FindStatObject("CoinStat").text = CoinManager.sharedInstance.GetCoinsCollected().ToString();
        FindStatObject("PowerupStat").text = powerupCount.ToString();
        FindStatObject("FlipStat").text = flipCount.ToString();
        FindStatObject("DashStat").text = dashCount.ToString();
        FindStatObject("DelayStat").text = delayCount.ToString();
        FindStatObject("NearMissStat").text = nearMissCount.ToString();
        FindStatObject("TimeStat").text = timeSurvived.ToString();
    }
    #region Helper Method for DisplayStats
    private TextMeshProUGUI FindStatObject(string tag) {
        return GameObject.FindGameObjectWithTag(tag).GetComponent<TextMeshProUGUI>();
    }
    #endregion

    //Public Methods
    public void AddPowerup() {
        powerupCount++;
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
}
