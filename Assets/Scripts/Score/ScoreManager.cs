using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager sharedInstance;

    private TextMeshProUGUI scoreDisplay = null;

    //Score Variables
    private int currentScore = 0;
    private bool increaseScore = true;

    //Counter Variables
    private float counter = 0f;
    private float counterMultiplier = 4f;
    private const float resetValue = 2 * Mathf.PI;

    #region OnSceneLoadDelegateCalls   
    private void OnEnable() {
        sharedInstance = this;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    #endregion
    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        if (scene.name == "PlayScene") {
            GameObject overlay = GameObject.Find("GameOverlay");
            scoreDisplay = overlay.transform.Find("ScoreDisplay").GetComponent<TextMeshProUGUI>();
            ResetScore();
            StartIncreasingScore();
        }
    }

    private void ResetScore() {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay() {
        scoreDisplay.text = currentScore.ToString();
    }

    private void Update() {
        if (increaseScore) {
            IncreaseScore();
        }
    }

    private void IncreaseScore() {
        counter += Time.deltaTime * counterMultiplier;
        if (counter >= resetValue) {
            counter -= resetValue;
            currentScore++;
            UpdateScoreDisplay();
        }
    }

    //Public Methods
    public void StartIncreasingScore() {
        increaseScore = true;
    }

    public void StopIncreasingScore() {
        increaseScore = false;
    }

    public int GetScore() {
        return currentScore;
    }
}
