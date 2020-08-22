using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable CompareOfFloatsByEqualityOperator
namespace UserInterface.Buttons {
    public class PauseButton : MonoBehaviour
    {
        //Reference Variables
        private GameObject gameOverlay;
        private GameObject pauseOverlay;

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
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootObject in rootGameObjects) {
                if (rootObject.CompareTag("OverlayFolder")) {
                    // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                    foreach (Canvas canvas in rootObject.GetComponentsInChildren(typeof(Canvas), true)) {
                        if (canvas.CompareTag("GameOverlay")) {
                            gameOverlay = canvas.gameObject;
                        } else if (canvas.CompareTag("PauseOverlay")) {
                            pauseOverlay = canvas.gameObject;
                        }
                    }
                }
            }
        }

        private void InitializedOverlays() {
            if (pausePart) {
                gameOverlay.SetActive(true);
                pauseOverlay.SetActive(false);
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
                    pauseOverlay.SetActive(true);
                    gameOverlay.SetActive(false);
                    _timeScale = Time.timeScale;
                    Time.timeScale = 0;
                    _paused = true;
                }
            } else {
                if (_paused) {
                    _paused = false;
                    Time.timeScale = _timeScale;
                    gameOverlay.SetActive(true);
                    pauseOverlay.SetActive(false);
                }
            }
        }
    }
}