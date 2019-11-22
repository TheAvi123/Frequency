using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //Reference Variables
    public static ScoreManager sharedInstance;

    //Configuration Parameters
    private const string hsSprite = "<sprite=\"StarSprite\" index=0>";

    //Score State Variabless
    private int highScore = 0;
    private int currentScore = 0;
    private bool increaseScore = false;

    private TextMeshProUGUI scoreDisplay = null;
    private TextMeshProUGUI hsDisplay = null;

    //Counter State Variables
    private float counter = 0f;
    private float counterMultiplier = 4f;
    private const float resetValue = 2 * Mathf.PI;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void OnSceneChange() {  
        if (SceneManager.GetActiveScene().name == "PlayScene") {
            FindScoreDisplay();
            ResetScore();
        }
        if (SceneManager.GetActiveScene().name == "GameOver") {
            FindScoreDisplay();
            UpdateScoreDisplay();
            FindHSDisplay();
            UpdateHS();
        }
        if (SceneManager.GetActiveScene().name == "Tutorial") {
            FindScoreDisplay();
            ResetScore();
        }
    }   //Called Through Singleton

    private void FindScoreDisplay() {
        if (gameObject.activeInHierarchy) {
            scoreDisplay = GameObject.FindGameObjectWithTag("ScoreDisplay").GetComponent<TextMeshProUGUI>();
        }
    }

    private void ResetScore() {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay() {
        if (gameObject.activeInHierarchy) {
            scoreDisplay.text = currentScore.ToString();
        }
    }

    private void FindHSDisplay() {
        if (gameObject.activeInHierarchy) {
            hsDisplay = GameObject.FindGameObjectWithTag("HighScoreDisplay").GetComponent<TextMeshProUGUI>();
        }
    }

    private void UpdateHS() {
        if (currentScore > highScore) {
            highScore = currentScore;
            UpdateHSDisplay(true);
        } else {
            UpdateHSDisplay(false);
        }
    }

    private void UpdateHSDisplay(bool newScore) {
        if (gameObject.activeInHierarchy) {
            if (newScore) {
                hsDisplay.text = hsSprite + " NEW RECORD " + hsSprite;
            } else {
                hsDisplay.text = hsSprite + " " + highScore.ToString() + " " + hsSprite;
            }
        }
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

    //Public Run Score Methods
    public void StartIncreasingScore() {
        increaseScore = true;
    }

    public void StopIncreasingScore() {
        increaseScore = false;
    }

    public int GetCurrentScore() {
        return currentScore;
    }

    public void AddScore(int amount) {
        currentScore += amount;
        UpdateScoreDisplay();
    }

    //Public HighScore Methods
    public void SetHighScore(int score) {
        highScore = score;
    }

    public int GetHighScore() {
        return highScore;
    }
}
