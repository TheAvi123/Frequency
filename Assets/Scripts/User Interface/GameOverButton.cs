using UnityEngine;

public class GameOverButton : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 1f;

    private void OnMouseDown() {
        GameStateManager.sharedInstance.GameOver(delayInSeconds);
    }
}
