using UnityEngine;

public class PauseButton : MonoBehaviour
{
    //Reference Variables
    [SerializeField] Canvas gameOverlay = null;
    [SerializeField] Canvas pauseOverlay = null;

    //Configuration Parameter
    [SerializeField] bool pauser = true;

    //State Variables
    private static bool paused = false;
    private static float timeScale = 1f;

    private void Awake() {
        if (pauser) {
            gameOverlay.gameObject.SetActive(true);
            pauseOverlay.gameObject.SetActive(false);
            paused = false;
            if (Time.timeScale != 1f) {
                Debug.LogError("Time Not At Regular Scale: " + Time.timeScale.ToString("F2"));
                Time.timeScale = 1f;
            }
        }
    }

    private void OnMouseDown() {
        if (pauser) {
            if (!paused) {
                pauseOverlay.gameObject.SetActive(true);
                gameOverlay.gameObject.SetActive(false);
                timeScale = Time.timeScale;
                Time.timeScale = 0;
                paused = true;
            }
        } else {
            if (paused) {
                paused = false;
                Time.timeScale = timeScale;
                gameOverlay.gameObject.SetActive(true);
                pauseOverlay.gameObject.SetActive(false);
            }
        }
    }
}