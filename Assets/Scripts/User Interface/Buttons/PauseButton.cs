using UnityEngine;

public class PauseButton : MonoBehaviour
{
    //Reference Variables
    [SerializeField] Canvas gameOverlay = null;
    [SerializeField] Canvas pauseOverlay = null;

    private void Awake() {
        gameOverlay.gameObject.SetActive(true);
        pauseOverlay.gameObject.SetActive(false);
    }

    private void OnMouseDown() {
        Time.timeScale = 0;
        gameOverlay.gameObject.SetActive(false);
        pauseOverlay.gameObject.SetActive(true);
    }
}