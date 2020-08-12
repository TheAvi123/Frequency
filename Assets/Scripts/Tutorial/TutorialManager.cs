using System.Collections;

using InputControllers;

using Player;

using UnityEngine;

namespace Tutorial {
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager sharedInstance;

        //Reference Variables
        private TutorialSegmentSpawner segmentSpawner;
        private GameObject gameOverlay;
        private GameObject pauseOverlay;
        private PlayerWave player;

        //State Variables
        private int currentStageIndex = 0;
        private bool waitingForFeedback = false;

        //Blocking Variables
        [Header("Abilities")]
        private bool flipEnabled = false;
        private bool dashEnabled = false;
        private bool delayEnabled = false;
        [Header("User Interface")]
        private bool scoreDisplayEnabled = false;
        private bool dashDisplayEnabled = false;
        private bool delayDisplayEnabled = false;

        //Internal Methods
        private void Awake() {
            SetSharedInstance();
            FindSegmentSpawner();
            FindOverlays();
            FindPlayer();
        }

        private void SetSharedInstance() {
            sharedInstance = this;
        }

        private void FindSegmentSpawner() {
            segmentSpawner = FindObjectOfType<TutorialSegmentSpawner>();
            if (!segmentSpawner) {
                Debug.LogError("No Segment Spawner Found In Tutorial Scene");
            }
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
            for (int i = 0; i < gameOverlay.transform.childCount; i++) {
                gameOverlay.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void SetPlayerDirection() {
            player.SetFrequencyToOne();
        }

        //Public Methods
        private void AttemptPlayerFlip() {
            if (flipEnabled) {
                player.Flip();
            }
        }

        private void AttemptPlayerDash() {
            if (dashEnabled) {
                player.Dash();
            }
        }

        private void AttemptPlayerDelay() {
            if (delayEnabled) {
                player.Delay();
            }
        }
    }
}
