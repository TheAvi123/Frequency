using System.Collections;

using InputControllers;

using Player;

using UnityEngine;

namespace Tutorial {
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager sharedInstance;

        //Reference Variables
        private GameObject gameOverlay;
        private GameObject pauseOverlay;
        private PlayerWave player;
        
        //Configuration Parameters
        [SerializeField] private float lerpDuration = 1.0f;

        //State Variables
        [Header("Time Variables")] 
        private Coroutine timeCoroutine;

        //Blocking Variables
        [Header("Abilities")] 
        private bool flipEnabled = false;
        private bool dashEnabled = false;
        private bool delayEnabled = false;
        private bool waitingOnTap = false;
        private bool waitingOnFlip = false;
        private bool waitingOnDash = false;
        private bool waitingOnDelay = false;
        
        [Header("User Interface")]
        private bool scoreDisplayEnabled = false;
        private bool dashDisplayEnabled = false;
        private bool delayDisplayEnabled = false;

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
                if (canvas.CompareTag("GameOverlay")) {
                    gameOverlay = canvas.gameObject;
                } else if (canvas.CompareTag("PauseOverlay")) {
                    pauseOverlay = canvas.gameObject;
                }
            }
            if (!gameOverlay || !pauseOverlay) {
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
            Transform overlay = gameOverlay.transform;
            for (int i = 0; i < overlay.childCount; i++) {
                overlay.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void SetPlayerDirection() {
            player.SetFrequencyToOne();
        }

        private IEnumerator LerpTimeScale(float delay, float targetScale) {
            yield return new WaitForSecondsRealtime(delay);
            float initialScale = Time.timeScale;
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < lerpDuration) {
                float lerpConstant = Mathf.Sqrt((Time.realtimeSinceStartup - startTime) / lerpDuration);
                Time.timeScale = Mathf.Lerp(initialScale, targetScale, lerpConstant);
                yield return null;
            }
            Time.timeScale = targetScale;
            timeCoroutine = null;
        }

        //Public Methods
        public void FreezeTime(float delay) {
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

        //Public Get & Set Methods
        public void SetFlipEnabled(bool status) {
            flipEnabled = status;
        }
        
        public void SetDashEnabled(bool status) {
            dashEnabled = status;
        }
        
        public void SetDelayEnabled(bool status) {
            delayEnabled = status;
        }
        
        public void SetScoreDisplayEnabled(bool status) {
            scoreDisplayEnabled = status;
        }
        
        public void SetDashDisplayEnabled(bool status) {
            dashDisplayEnabled = status;
        }
        
        public void SetDelayDisplayEnabled(bool status) {
            delayDisplayEnabled = status;
        }
        
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

        //Ability Methods
        private void AttemptPlayerFlip() {
            if (waitingOnTap && timeCoroutine == null) {
                waitingOnTap = false;
                ResumeTime();
            } else if (waitingOnFlip && timeCoroutine == null) {
                waitingOnFlip = false;
                ResumeTime();
                player.Flip();
            } else if (flipEnabled) {
                player.Flip();
            }
        }

        private void AttemptPlayerDash() {
            if (waitingOnDash && timeCoroutine == null) {
                waitingOnDash = false;
                ResumeTime();
                player.Dash();
            } else if (dashEnabled) {
                player.Dash();
            }
        }

        private void AttemptPlayerDelay() {
            if (waitingOnDelay && timeCoroutine == null) {
                waitingOnDelay = false;
                player.Delay();
                ResumeTime();
            } else if (delayEnabled) {
                player.Delay();
            }
        }
    }
}
