using System.Collections;

using Systems;

using InputControllers;

using Player;

using UnityEngine;

namespace Tutorial {
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager sharedInstance;

        //Reference Variables
        private Canvas gameOverlay;
        private PlayerWave player;
        
        //State Variables
        [Header("Time Variables")] 
        private Coroutine timeCoroutine;
        private float lerpDuration = 0.5f;

        //Blocking Variables
        private bool waitingOnTap = false;
        private bool waitingOnFlip = false;
        private bool waitingOnDash = false;
        private bool waitingOnDelay = false;

        //Internal Methods
        private void Awake() {
            SetSharedInstance();
            FindOverlays();
            FindPlayer();
        }

        private void SetSharedInstance() {
            sharedInstance = this;
        }
        
        private void FindOverlays() {
            Canvas[] canvasList = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (Canvas canvas in canvasList) {
                if (canvas.CompareTag("GameOverlay") && canvas.gameObject.activeInHierarchy) {
                    gameOverlay = canvas;
                }
            }
            if (!gameOverlay) {
                Debug.LogError("Canvas or Game Overlay Not Found In Tutorial Scene");
            }
        }

        private void FindPlayer() {
            player = FindObjectOfType<PlayerWave>();
            if (!player) {
                Debug.LogError("No Player Found In Tutorial Scene");
                enabled = false;
            }
        }

        private void Start() {
            SetInputControllerActions();
            StartCoroutine(DisableInterfaceElements());
            SetPlayerDirection();
        }

        private void SetInputControllerActions() {
            InputController inputController = FindObjectOfType<InputController>();
            if (!inputController) {
                gameObject.SetActive(false);
            } else {
                inputController.SetTapAction(AttemptPlayerFlip);
                inputController.SetUpSwipeAction(AttemptPlayerDash);
                inputController.SetDownSwipeAction(AttemptPlayerDelay);
            }
        }

        private IEnumerator DisableInterfaceElements() {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Transform overlay = gameOverlay.transform;
            for (int i = 0; i < overlay.childCount; i++) {
                overlay.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void SetPlayerDirection() {
            player.SetFrequencyDirection(false);
        }

        private IEnumerator LerpTimeScale(float delay, float targetScale) {
            yield return new WaitForSecondsRealtime(delay);
            float initialScale = Time.timeScale;
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime <= lerpDuration) {
                float lerpConstant = Mathf.Sqrt((Time.realtimeSinceStartup - startTime) / lerpDuration);
                Time.timeScale = Mathf.Lerp(initialScale, targetScale, lerpConstant);
                yield return null;
            }
            Time.timeScale = targetScale;
            timeCoroutine = null;
        }

        //Public Methods
        public void FreezeTime(float delay, float triggerLerpDuration) {
            lerpDuration = triggerLerpDuration;
            if (timeCoroutine == null) {
                timeCoroutine = StartCoroutine(LerpTimeScale(delay, 0f));
            } else {
                Debug.LogWarning("Time Coroutine Already In Progress");
                StopCoroutine(timeCoroutine);
                timeCoroutine = StartCoroutine(LerpTimeScale(delay, 0f));
            }
        }

        public void ResumeTime() {
            if (timeCoroutine == null) {
                timeCoroutine = StartCoroutine(LerpTimeScale(0f, 1f));
            } else {
                Debug.LogWarning("Time Coroutine Already In Progress");
                StopCoroutine(timeCoroutine);
                timeCoroutine = StartCoroutine(LerpTimeScale(0f, 1f));
            }
        }
        
        public void EndTutorial() {
            float endDuration = 5f;
            float offsetDuration = endDuration - 1;
            ChangeCameraOffset(new Vector2(0f, 1.1f), offsetDuration);
            StartCoroutine(ReturnToMenu(endDuration, offsetDuration));
        }
        
        //Helper Methods
        private void ChangeCameraOffset(Vector2 offsetCoordinates, float changeDuration) {
            PlayerFollow camFollow = Camera.main.GetComponent<PlayerFollow>();
            if (camFollow) {
                camFollow.ChangeCameraOffset(offsetCoordinates, changeDuration);
            } else {
                Debug.LogWarning("No PlayerFollow Script Found On Main Camera In Tutorial");
            }
        }

        private IEnumerator ReturnToMenu(float endDuration, float offsetDuration) {
            yield return new WaitForSeconds(offsetDuration);
            PlayerFollow camFollow = Camera.main.GetComponent<PlayerFollow>();
            if (camFollow) {
                camFollow.StopFollowingPlayer();
            } else {
                Debug.LogWarning("No PlayerFollow Script Found On Main Camera In Tutorial");
            }
            yield return new WaitForSeconds(endDuration - offsetDuration);
            GameStateManager.sharedInstance.PlayGame();
        }

        //Ability Methods
        private void AttemptPlayerFlip() {
            if (waitingOnTap && timeCoroutine == null) {
                waitingOnTap = false;
                ResumeTime();
            } else if (waitingOnFlip && timeCoroutine == null) {
                waitingOnFlip = false;
                ResumeTime();
                player.Flip();
            }
        }

        private void AttemptPlayerDash() {
            if (waitingOnDash && timeCoroutine == null) {
                waitingOnDash = false;
                ResumeTime();
                player.Dash();
            }
        }

        private void AttemptPlayerDelay() {
            if (waitingOnDelay && timeCoroutine == null) {
                waitingOnDelay = false;
                player.Delay();
                ResumeTime();
            }
        }
        
        //Public Segment Methods
        public void WaitForTap() {
            waitingOnTap = true;
        }
        
        public void WaitForFlip() {
            waitingOnFlip = true;
        }
        
        public void WaitForDash() {
            waitingOnDash = true;
        }
        
        public void WaitForDelay() {
            waitingOnDelay = true;
        }

        public void EnableDashDisplay() {
            gameOverlay.transform.Find("DashDisplay").gameObject.SetActive(true);
        }
        
        public void EnableDelayDisplay() {
            gameOverlay.transform.Find("DelayDisplay").gameObject.SetActive(true);
        }

        public void EnableScoreDisplay() {
            gameOverlay.transform.Find("ScoreDisplay").gameObject.SetActive(true);
        }
        
        public void EnableCoinDisplay() {
            gameOverlay.transform.Find("CoinDisplay").gameObject.SetActive(true);
        }
    }
}
