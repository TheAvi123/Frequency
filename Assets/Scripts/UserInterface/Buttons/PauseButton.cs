using UnityEngine;

// ReSharper disable CompareOfFloatsByEqualityOperator
namespace UserInterface.Buttons {
    public class PauseButton : MonoBehaviour
    {
        //Reference Variables
        private Canvas gameOverlay;
        private Canvas pauseOverlay;

        //Configuration Parameter
        [SerializeField] bool pausePart = true;

        //State Variables
        private static bool _paused = false;
        private static float _timeScale = 1f;

        private void Awake() {
            FindOverlays();
            InitializedOverlays();
        }

        private void FindOverlays() {
            Canvas[] canvasList = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (Canvas canvas in canvasList) {
                if (canvas.CompareTag("GameOverlay")) {
                    gameOverlay = canvas;
                } else if (canvas.CompareTag("PauseOverlay")) {
                    pauseOverlay = canvas;
                }
            }
        }

        private void InitializedOverlays() {
            if (pausePart) {
                gameOverlay.gameObject.SetActive(true);
                pauseOverlay.gameObject.SetActive(false);
                _paused = false;
                if (Time.timeScale != 1f) {
                    Debug.LogError("Time Not At Regular Scale: " + Time.timeScale.ToString("F2"));
                    Time.timeScale = 1f;
                }
            }
        }

        private void OnMouseDown() {
            if (pausePart) {
                if (!_paused) {
                    pauseOverlay.gameObject.SetActive(true);
                    gameOverlay.gameObject.SetActive(false);
                    _timeScale = Time.timeScale;
                    Time.timeScale = 0;
                    _paused = true;
                }
            } else {
                if (_paused) {
                    _paused = false;
                    Time.timeScale = _timeScale;
                    gameOverlay.gameObject.SetActive(true);
                    pauseOverlay.gameObject.SetActive(false);
                }
            }
        }
    }
}