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
    private int[] highScores = new int[] {0, 0, 0, 0, 0, 0, 0};
    private int currentScore = 0;
    private bool increaseScore = false;

    private TextMeshProUGUI scoreDisplay = null;
    private TextMeshProUGUI highScoreDisplay = null;

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
            FindHighScoreDisplay();
            UpdateHighScores();
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

    private void FindHighScoreDisplay() {
        if (gameObject.activeInHierarchy) {
            highScoreDisplay = GameObject.FindGameObjectWithTag("HighScoreDisplay").GetComponent<TextMeshProUGUI>();
        }
    }

    private void UpdateHighScores() {
        if (currentScore > highScores[0]) {
            UpdateHighScoreArray(currentScore);
            UpdateHighScoreDisplay(true);
        } else if (currentScore > highScores[highScores.Length - 1]) {
            UpdateHighScoreArray(currentScore);
            UpdateHighScoreDisplay(false);
        } else {
            UpdateHighScoreDisplay(false);
        }
    }
    #region Helper Method for UpdateHighScores
    private void UpdateHighScoreArray(int newScore) {
        for(int i = 0; i < highScores.Length; i++) {
            if (newScore > highScores[i]) {
                int temp = highScores[i];
                highScores[i] = newScore;
                newScore = temp;
            }
        }
    }
    #endregion

    private void UpdateHighScoreDisplay(bool newRecord) {
        if (gameObject.activeInHierarchy) {
            if (newRecord) {
                highScoreDisplay.text = hsSprite + " NEW RECORD " + hsSprite;
            } else {
                highScoreDisplay.text = hsSprite + " " + highScores[0].ToString() + " " + hsSprite;
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
    public void SetHighScores(int[] highScores) {
        this.highScores = highScores;
    }

    public int[] GetHighScores() {
        return highScores;
    }
}
