using System;
using System.Collections;

using Systems;

using CameraFeatures;

using Collectables;

using InputControllers;

using Statistics;

using UnityEngine;

using UserInterface;

namespace Player {
    public class PlayerInteractions : MonoBehaviour
    {
        //Reference Variables
        private Transform player;

        //Configuration Parameters
        [Header("Death")]
        [SerializeField] float deathFreezeTime = 0.75f;
        [SerializeField] float playerDeathDelay = 2.5f;
        
        [Header("Visual Effects")]
        [SerializeField] GameObject plusOneVFX = null;
        [SerializeField] ParticleSystem deathVFX = null;

        //State Variables
        private bool playerAlive = true;

        //Modifier Variables
        private bool ghostMode = false;

        //Internal Methods
        private void Awake() {
            GetPlayerTransform();
        }

        private void GetPlayerTransform() {
            player = gameObject.transform;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            switch (other.tag) {
                case "ObstaclePart":
                    ObstacleCollision();
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            switch (other.tag) {
                case "NMCollider":
                    NearMissTrigger();
                    break;
            }
        }

        private void ObstacleCollision() {
            if (ghostMode) {
                //Do Nothing
            } else {
                StartCoroutine(PlayerDied());
            }
        }

        private void NearMissTrigger() {
            if (ghostMode) {
                //Do Nothing
            } else {
                NearMiss();
            }
        }

        private IEnumerator PlayerDied() {
            playerAlive = false;
            DisablePlayer();
            DisableInputControllers();
            StopIncreasingScore();
            ClearInfoDisplays();
            RemoveModifiers();
            DisablePause();
            yield return StartCoroutine(WaitForDeathFreeze());
            SpawnDeathVFX();
            ShakeScreen();
            LoadGameOver();
        }
        #region PlayerDeath Helper Functions
        private void DisablePlayer() {
            gameObject.GetComponent<PlayerWave>().enabled = false;
            gameObject.GetComponent<PlayerAbilityManager>().enabled = false;
            gameObject.GetComponent<PlayerDirection>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }

        private void DisableInputControllers() {
            TouchController touchController = FindObjectOfType<TouchController>();
            MouseController mouseController = FindObjectOfType<MouseController>();
            if (touchController) {
                touchController.gameObject.SetActive(false);
            } else if (mouseController) {
                mouseController.gameObject.SetActive(false);
            }
        }

        private void StopIncreasingScore() {
            ScoreManager.sharedInstance.StopIncreasingScore();
        }

        private void ClearInfoDisplays() {
            InfoDisplayer.sharedInstance.ClearDisplays();
        }

        private void RemoveModifiers() {
            ModifierManager.sharedInstance.EndModifierEffects();
        }

        private void DisablePause() {
            RectTransform[] interfaceElements = Resources.FindObjectsOfTypeAll<RectTransform>();
            foreach (RectTransform element in interfaceElements) {
                if (element.gameObject.name == "PauseCollider") {
                    element.gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator WaitForDeathFreeze() {
            Time.timeScale = 0f;
            Camera.main.gameObject.GetComponent<PlayerFollow>().DeathZoomAnimation(deathFreezeTime, player.position);
            yield return new WaitForSecondsRealtime(deathFreezeTime);
            Time.timeScale = 1f;
        }

        private void SpawnDeathVFX() {
            Instantiate(deathVFX, player.position, player.rotation);
            player.GetChild(0).gameObject.SetActive(false);
            player.GetChild(1).gameObject.SetActive(false);
        }

        private void ShakeScreen() {
            CameraShaker.sharedInstance.AddCameraShake(1f);
        }

        private void LoadGameOver() {
            GameStateManager.sharedInstance.GameOver(playerDeathDelay);
        }
        #endregion

        private void NearMiss() {
            if (playerAlive) {
                ScoreManager.sharedInstance.AddScore(1);
                StatsManager.sharedInstance.AddNearMiss();
                InfoDisplayer.sharedInstance.DisplayInfo("NEAR MISS");
                SpawnPlusOneSprite();
            }
        }
        #region NearMiss Helper Functions
        private void SpawnPlusOneSprite() {
            GameObject plusOne = Instantiate(plusOneVFX, player.position, Quaternion.identity);
            Destroy(plusOne, 2f);
        }
        #endregion

        //Public Methods
        public void SetGhostMode(bool status) {
            ghostMode = status;
        }
    }
}
