﻿using System.Diagnostics.CodeAnalysis;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statistics {
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager sharedInstance;

        //Configuration Parameters
        private const string StarSprite = "<sprite=\"StarSprite\" index=0>";

        //Score State Variables
        private int currentScore = 0;
        private bool increaseScore = false;
        private int[] highScores = new int[] { 0, 0, 0, 0, 0, 0, 0 };

        private TextMeshProUGUI scoreDisplay = null;
        private TextMeshProUGUI highScoreDisplay = null;

        //Counter State Variables
        private float counter = 0f;
        private float counterMultiplier = 4f;
        private const float ResetValue = 2 * Mathf.PI;

        //Modifier Variables
        private bool doubleScore = false;
        private bool freezeScore = false;

        //Internal Methods
        private void Awake() {
            SetSharedInstance();
        }

        private void SetSharedInstance() {
            sharedInstance = this;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnSceneChange() {  
            if (SceneManager.GetActiveScene().name == "PlayScene") {
                FindScoreDisplay();
                ResetScore();
                ResetModifierStates();
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

        private void ResetModifierStates() {
            doubleScore = false;
            freezeScore = false;
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
        #region Helper Methods for UpdateHighScores
        private void UpdateHighScoreArray(int newScore) {
            for(int i = 0; i < highScores.Length; i++) {
                if (newScore > highScores[i]) {
                    int temp = highScores[i];
                    highScores[i] = newScore;
                    newScore = temp;
                }
            }
        }

        private void UpdateHighScoreDisplay(bool newRecord) {
            if (gameObject.activeInHierarchy) {
                if (newRecord) {
                    highScoreDisplay.text = StarSprite + " NEW RECORD " + StarSprite;
                } else {
                    highScoreDisplay.text = StarSprite + " " + highScores[0].ToString() + " " + StarSprite;
                }
            }
        }
        #endregion

        private void Update() {
            if (increaseScore) {
                IncreaseScore();
            }
        }

        private void IncreaseScore() {
            IncreaseCounter();
            if (counter >= ResetValue) {
                counter -= ResetValue;
                currentScore++;
                UpdateScoreDisplay();
            }
        }

        private void IncreaseCounter() {
            if (doubleScore) {
                counter += Time.deltaTime * counterMultiplier * 2;
            } else if (freezeScore) {
                //Don't increase counter
            } else {
                counter += Time.deltaTime * counterMultiplier;
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
        public void SetHighScores(int[] loadedHighScores) {
            highScores = loadedHighScores;
        }

        public int[] GetHighScores() {
            return highScores;
        }

        //Public Modifier Methods
        public void SetDoubleScore(bool status) {
            doubleScore = status;
        }

        public void SetFreezeScore(bool status) {
            freezeScore = status;
        }
    }
}
