using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    //Reference Variables
    [SerializeField] Canvas gameOverlay = null;
    [SerializeField] Canvas pauseOverlay = null;

    private void OnMouseDown() {
        gameOverlay.gameObject.SetActive(true);
        pauseOverlay.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}